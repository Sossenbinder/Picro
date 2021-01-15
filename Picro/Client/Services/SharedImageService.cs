using Microsoft.JSInterop;
using Picro.Client.Communication.Interface;
using Picro.Client.Services.Interface;
using Picro.Client.Utils;
using Picro.Common.Eventing.Events;
using Picro.Common.Eventing.Notifications;
using Picro.Common.Utils.Tasks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Picro.Common.Web.Extensions;
using Picro.Module.Image.DataTypes.Notification;

namespace Picro.Client.Services
{
	public class SharedImageService : ISharedImageService
	{
		public AsyncEvent ImageReceived { get; } = new();

		public List<ImageShareInfo> SharedImages { get; }

		private readonly IJSRuntime _jsRuntime;

		private readonly HttpClient _httpClient;

		private readonly IRequestMessageFactory _requestMessageFactory;

		public SharedImageService(
			IKeepAliveService keepAliveService,
			IJSRuntime jsRuntime,
			IHttpClientFactory httpClientFactory,
			IRequestMessageFactory requestMessageFactory)
		{
			_jsRuntime = jsRuntime;
			_requestMessageFactory = requestMessageFactory;

			SharedImages = new List<ImageShareInfo>();

			_httpClient = httpClientFactory.CreateClient(HttpClients.PicroBackend);

			keepAliveService.RegisterHandler<ImageShareInfo>(NotificationType.ImageShared, OnNewImageReceived);
		}

		public async Task InitializeSharedImages()
		{
			var message = _requestMessageFactory.Create(HttpMethod.Get, $"/Image/GetReceivedImages");

			var response = await _httpClient.SendAsync(message);

			if (response.IsSuccessStatusCode)
			{
				var jsonResponse = await response.GetJsonResponse<IEnumerable<ImageShareInfo>>();

				if (jsonResponse.Success)
				{
					SharedImages.AddRange(jsonResponse.Data!);
				}
			}
		}

		private async Task OnNewImageReceived(FrontendNotification<ImageShareInfo> notification)
		{
			FireAndForgetTask.Run(() => AcknowledgeReceival(notification.Data.ImageId), null);

			await _jsRuntime.InvokeAsync<object>("notification.showNotification", null);
			await ImageReceived.Raise();
		}

		private async Task AcknowledgeReceival(Guid imageId)
		{
			var message = _requestMessageFactory.Create(HttpMethod.Post, $"/Ack/AckImageShare?imageId={imageId}");

			await _httpClient.SendAsync(message);
		}
	}
}