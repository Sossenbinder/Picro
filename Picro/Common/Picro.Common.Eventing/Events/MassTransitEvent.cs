using MassTransit;
using MassTransit.SignalR.Utils;
using Microsoft.Extensions.Logging;
using Picro.Common.Disposable;
using Picro.Common.Eventing.DataTypes;
using Picro.Common.Eventing.Events.Interface;
using Picro.Common.Eventing.Events.MassTransit.Interface;
using Picro.Common.Extensions;
using Picro.Common.Extensions.Async;
using Picro.Common.Utils.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Picro.Common.Eventing.Events
{
	/// <summary>
	/// Event hub which registers itself as IConsumer and then distributes events based on service-local registrations.
	/// This way, there is a more fine-grained control for disposal and lifetimes, as MT does not support
	/// connecting / disconnecting on the fly
	/// </summary>
	/// <typeparam name="TEventArgs"></typeparam>
	public class MassTransitEvent<TEventArgs> : IDistributedEvent<TEventArgs>, IConsumer<TEventArgs>
		where TEventArgs : class
	{
		private readonly string _queueName;

		private readonly IMassTransitEventingService _massTransitEventingService;

		private readonly ILogger _logger;

		private readonly ConcurrentHashSet<Func<TEventArgs, Task>> _handlers = new();

		private readonly ConcurrentHashSet<Func<Fault<TEventArgs>, Task>> _faultHandlers = new();

		private readonly object _registrationLock = new();

		public MassTransitEvent(
			string queueName,
			IMassTransitEventingService massTransitEventingService,
			ILogger logger)
		{
			_queueName = queueName;
			_massTransitEventingService = massTransitEventingService;
			_logger = logger;

			_massTransitEventingService.RegisterConsumer(queueName, this);
		}

		public DisposableAction Register(Func<TEventArgs, Task> handler)
		{
			_handlers.Add(handler);

			return new DisposableAction(() => UnRegister(handler));
		}

		public void UnRegister(Func<TEventArgs, Task> handler)
		{
			_handlers.Remove(handler);
		}

		internal List<Func<TEventArgs, Task>> GetAllRegisteredEvents()
		{
			return _handlers.ToArray().ToList();
		}

		public async Task Consume(ConsumeContext<TEventArgs> context)
		{
			try
			{
				var message = context.Message;
				await _handlers.ToArray().ParallelAsync(handler => handler(message));
			}
			catch (Exception e)
			{
				_logger.LogException(e);

				// It is important to throw here, as a consumer registration will raise an {queueName}_error event
				// when an exception is passed to the calling MassTransit code!
				throw;
			}
		}

		public void RaiseFireAndForget(TEventArgs eventArgs)
		{
			_ = FireAndForgetTask.Run(() => _massTransitEventingService.RaiseEvent(eventArgs), _logger);
		}

		public DisposableAction RegisterForErrors(Func<Fault<TEventArgs>, Task> faultHandler)
		{
			_faultHandlers.Add(faultHandler);

			lock (_registrationLock)
			{
				_massTransitEventingService.RegisterForEvent<Fault<TEventArgs>>($"{_queueName}", OnErrorReceived, QueueType.ErrorQueue);
			}

			return new DisposableAction(() => _faultHandlers.Remove(faultHandler));
		}

		public Task Raise(TEventArgs eventArgs)
		{
			return _massTransitEventingService.RaiseEvent(eventArgs);
		}

		private async Task OnErrorReceived(Fault<TEventArgs> eventArgs)
		{
			try
			{
				await _faultHandlers.ToArray().ParallelAsync(handler => handler(eventArgs));
			}
			catch (Exception e)
			{
				_logger.LogException(e);
			}
		}
	}
}