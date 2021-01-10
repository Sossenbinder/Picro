using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Picro.Common.Eventing.Notifications;
using Picro.Module.Notification.Service.Interface;
using Picro.Module.User.DataTypes;
using WebPush;

namespace Picro.Module.Notification.Service
{
    public class WebPushService : IWebPushService
    {
        private readonly WebPushClient _webPushClient;

        private readonly VapidDetails _vapidDetails;

        public WebPushService(
            WebPushClient webPushClient,
            VapidDetails vapidDetails)
        {
            _webPushClient = webPushClient;
            _vapidDetails = vapidDetails;
        }

        public async Task SendNotificationToSession<T>(NotificationSubscription subscription, FrontendNotification<T> notification)
        {
            var pushSubscription = new PushSubscription(subscription.Url, subscription.P256dh, subscription.Auth);
            try
            {
                var payload = JsonConvert.SerializeObject(notification);
                await _webPushClient.SendNotificationAsync(pushSubscription, payload, _vapidDetails);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error sending push notification: " + ex.Message);
            }
        }
    }
}