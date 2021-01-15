using MassTransit;
using MassTransit.SignalR.Contracts;
using MassTransit.SignalR.Utils;
using Microsoft.AspNetCore.SignalR.Protocol;
using Picro.Common.Eventing.Events.MassTransit.Interface;
using Picro.Common.Eventing.Notifications;
using Picro.Common.SignalR.Hubs;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Picro.Common.Eventing.Events.MassTransit
{
	public class MassTransitSignalRBackplaneService : IMassTransitSignalRBackplaneService
	{
		[NotNull]
		private readonly IBusControl _busControl;

		[NotNull]
		private readonly List<IHubProtocol> _signalRProtocols;

		public MassTransitSignalRBackplaneService([NotNull] IBusControl busControl)
		{
			_busControl = busControl;

			_signalRProtocols = new List<IHubProtocol>() { new JsonHubProtocol() };
		}

		public async Task RaiseAllSignalREvent<T>(FrontendNotification<T> notification, string[]? excludedConnectionIds = null)
		{
			var signalRParams = new
			{
				ExcludedConnectionIds = excludedConnectionIds,
				Messages = CreateProtocolDict(notification)
			};

			await _busControl.Publish<All<SessionHub>>(signalRParams);
		}

		public async Task RaiseConnectionSignalREvent<T>(string connectionId, FrontendNotification<T> notification)
		{
			var signalRParams = new
			{
				ConnectionId = connectionId,
				Messages = CreateProtocolDict(notification)
			};

			await _busControl.Publish<Connection<SessionHub>>(signalRParams);
		}

		public async Task RaiseGroupSignalREvent<T>(string groupName, FrontendNotification<T> notification, string[]? excludedConnectionIds = null)
		{
			var signalRParams = new
			{
				GroupName = groupName,
				ExcludedConnectionIds = excludedConnectionIds,
				Messages = CreateProtocolDict(notification)
			};

			await _busControl.Publish<Group<SessionHub>>(signalRParams);
		}

		public async Task RaiseUserSignalREvent<T>(string userId, FrontendNotification<T> notification)
		{
			var signalRParams = new
			{
				UserId = userId,
				Messages = CreateProtocolDict(notification)
			};

			await _busControl.Publish<User<SessionHub>>(signalRParams);
		}

		private IReadOnlyDictionary<string, byte[]> CreateProtocolDict<T>(FrontendNotification<T> notification)
		{
			return _signalRProtocols.ToProtocolDictionary(
				notification.NotificationType.ToString(),
				GetPayload(notification));
		}

		private static object[] GetPayload<T>(FrontendNotification<T> notification)
		{
			var payload = new object[]
			{
				new
				{
					operation = notification.Operation,
					data = notification.Data,
				}
			};

			return payload;
		}
	}
}