using System;
using System.Threading.Tasks;
using Picro.Client.Communication.Interface;
using Picro.Client.Services.Interface;
using Picro.Common.Eventing.Events;
using Picro.Common.Eventing.Notifications;
using Picro.Module.Image.DataTypes.Notification;

namespace Picro.Client.Services
{
    public class DistributedImageService : IDistributedImageService
    {
        public AsyncEvent ImageReceived { get; } = new();

        public DistributedImageService(IKeepAliveService keepAliveService)
        {
            keepAliveService.RegisterHandler<ImageShareInfo>(NotificationType.ImageShared, OnNewImageReceived);
        }

        private async Task OnNewImageReceived(FrontendNotification<ImageShareInfo> notification)
        {
            Console.WriteLine(notification);

            await ImageReceived.Raise();
        }
    }
}