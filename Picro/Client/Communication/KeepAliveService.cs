using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Picro.Client.Communication.Interface;
using Picro.Common.Eventing.Notifications;
using Picro.Common.Extensions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Picro.Client.Communication
{
	public class KeepAliveService : IKeepAliveService
	{
		private readonly HubConnection _hubConnection;

		public KeepAliveService(IConfiguration configuration)
		{
			_hubConnection = new HubConnectionBuilder()
				.WithUrl($"{configuration["RemoteEndpoint"]}/SessionHub")
				.Build();
		}

		public async Task InitializeConnection()
		{
			try
			{
				await _hubConnection.StartAsync();
			}
			catch (HttpRequestException)
			{
				Console.WriteLine("Failed to initialize websocket connection, will revert to long polling...");
			}
		}

		public IDisposable RegisterHandler<T>(NotificationType notificationType, Action<FrontendNotification<T>> handler)
			=> RegisterHandler(notificationType, handler.MakeTaskCompatible()!);

		public IDisposable RegisterHandler<T>(NotificationType notificationType, Func<FrontendNotification<T>, Task> handler)
		{
			return _hubConnection.On(notificationType.ToString(), handler);
		}

		public void RemoveAllHandlers(NotificationType notificationType)
		{
			_hubConnection.Remove(notificationType.ToString());
		}
	}
}