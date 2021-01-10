using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Picro.Client.Communication.Interface;
using Picro.Client.Services.Interface;
using Picro.Client.Utils;
using Picro.Common.Eventing.Events;
using Picro.Common.Eventing.Notifications;
using Picro.Common.Utils.Tasks;
using Picro.Module.Image.DataTypes.Notification;

namespace Picro.Client.Services
{
    public class DistributedImageService : IDistributedImageService
    {
        public AsyncEvent ImageReceived { get; } = new();

        private readonly IJSRuntime _jsRuntime;

        private readonly HttpClient _httpClient;

        private readonly IRequestMessageFactory _requestMessageFactory;

        public DistributedImageService(
            IKeepAliveService keepAliveService,
            IJSRuntime jsRuntime,
            IHttpClientFactory httpClientFactory,
            IRequestMessageFactory requestMessageFactory)
        {
            _jsRuntime = jsRuntime;
            _requestMessageFactory = requestMessageFactory;

            _httpClient = httpClientFactory.CreateClient(HttpClients.PicroBackend);

            keepAliveService.RegisterHandler<ImageShareInfo>(NotificationType.ImageShared, OnNewImageReceived);
        }

        private async Task OnNewImageReceived(FrontendNotification<ImageShareInfo> notification)
        {
            FireAndForgetTask.Run(() => AcknowledgeReceival(notification.Data.ImageId), null);

            await _jsRuntime.InvokeAsync<object>("notification.showNotification", null);
            Console.WriteLine(notification);

            await ImageReceived.Raise();
        }

        private async Task AcknowledgeReceival(Guid imageId)
        {
            var message = _requestMessageFactory.Create(HttpMethod.Post, $"/Ack/AckImageShare?imageId={imageId}");

            await _httpClient.SendAsync(message);
        }
    }
}