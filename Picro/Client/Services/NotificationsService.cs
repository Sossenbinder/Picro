using Microsoft.JSInterop;
using Picro.Client.Communication.Interface;
using Picro.Client.Services.Interface;
using Picro.Client.Utils;
using Picro.Module.User.DataTypes;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Picro.Client.Services
{
	public class NotificationsService : INotificationsService
	{
		private readonly IJSRuntime _jsRuntime;

		private readonly HttpClient _httpClient;

		private readonly IRequestMessageFactory _requestMessageFactory;

		public NotificationsService(
			IJSRuntime jsRuntime,
			IHttpClientFactory httpClientFactory,
			IRequestMessageFactory requestMessageFactory)
		{
			_jsRuntime = jsRuntime;
			_requestMessageFactory = requestMessageFactory;
			_httpClient = httpClientFactory.CreateClient(HttpClients.PicroBackend);
		}

		public async Task RequestNotificationSubscription()
		{
			var subscription = await _jsRuntime.InvokeAsync<NotificationSubscription?>("notification.requestSubscription", null);

			if (subscription != null)
			{
				var requestMessage = _requestMessageFactory.Create(HttpMethod.Post, "/Notifications/Register");

				var jsonContent = JsonContent.Create(subscription);

				requestMessage.Content = jsonContent;

				await _httpClient.SendAsync(requestMessage);
			}
		}
	}
}