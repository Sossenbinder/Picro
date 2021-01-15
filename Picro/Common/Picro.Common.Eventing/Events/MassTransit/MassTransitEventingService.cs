using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Logging;
using Picro.Common.Eventing.DataTypes;
using Picro.Common.Eventing.Events.MassTransit.Interface;
using Picro.Common.Extensions;
using Picro.Common.Utils.Retry;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Picro.Common.Eventing.Events.MassTransit
{
	public class MassTransitEventingService : IMassTransitEventingService
	{
		private readonly ILogger _logger;

		private readonly IBusControl _busControl;

		public MassTransitEventingService(
			ILogger<MassTransitEventingService> logger,
			IBusControl busControl)
		{
			_logger = logger;
			_busControl = busControl;

			var _ = Start();
		}

		public async Task Start()
		{
			var timeBetween = TimeSpan.FromSeconds(5);

			await RetryStrategy.DoRetryExponential(() =>
			{
				_busControl.Start();

				_logger.LogInformation("Successfully initiated RabbitMQ EventBus.");
			}, retryCount =>
			{
				_logger.LogInformation($"Retrying rabbitMQ start for the {retryCount}# time");
				return Task.CompletedTask;
			}, timeBetween.Seconds);
		}

		public Task RaiseEvent<T>(T message)
			where T : class
		{
			return _busControl.Publish(message);
		}

		private void RegisterForEvent<T>(string queueName, Func<ConsumeContext<T>, Task> handler, QueueType queueType = QueueType.RegularQueue)
			where T : class
		{
			RegisterOnQueue(queueName, ep => ep.Handler<T>(ctx => handler(ctx)), queueType);
		}

		public void RegisterForEvent<T>(string queueName, Action<T> handler, QueueType queueType = QueueType.RegularQueue)
			where T : class
		{
			RegisterForEvent(queueName, handler.MakeTaskCompatible()!, queueType);
		}

		public void RegisterForEvent<T>(string queueName, Func<T, Task> handler, QueueType queueType = QueueType.RegularQueue) where T : class
		{
			RegisterForEvent<T>(queueName, ctx => handler(ctx.Message), queueType);
		}

		public void RegisterConsumer<TConsumer>(string queueName, TConsumer consumer, QueueType queueType = QueueType.RegularQueue)
			where TConsumer : class, IConsumer
		{
			RegisterOnQueue(queueName, ep => ep.Instance(consumer), queueType);
		}

		private void RegisterOnQueue(string queueName, Action<IReceiveEndpointConfigurator> registrationCb, QueueType queueType = QueueType.RegularQueue)
		{
			var serviceIdentity = Assembly.GetEntryAssembly()?.GetName().Name ?? Dns.GetHostName();
			queueName = $"{queueName}_{serviceIdentity}";

			if (queueType == QueueType.ErrorQueue)
			{
				queueName = $"{queueName}_error";
			}

			_busControl.ConnectReceiveEndpoint(queueName, ep =>
			{
				ep.UseMessageRetry(retry => retry.Exponential(
					10,
					TimeSpan.FromSeconds(2),
					TimeSpan.FromMinutes(5),
					TimeSpan.FromSeconds(10)));

				registrationCb(ep);
			});
		}
	}
}