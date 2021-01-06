using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Picro.Common.Eventing.Notifications;
using Picro.Module.Notification.DataTypes;
using Picro.Module.Notification.Service.Interface;
using WebPush;

namespace Picro.Module.Notification.Service
{
    public class WebPushService : IWebPushService
    {
        public async Task SendNotificationToSession(NotificationSubscription subscription, FrontendNotification<string> notification)
        {
            // For a real application, generate your own
            var publicKey = "BLC8GOevpcpjQiLkO7JmVClQjycvTCYWm6Cq_a7wJZlstGTVZvwGFFHMYfXt6Njyvgx_GlXJeo5cSiZ1y4JOx1o";
            var privateKey = "OrubzSz3yWACscZXjFQrrtDwCKg-TGFuWhluQ2wLXDo";

            var pushSubscription = new PushSubscription(subscription.Url, subscription.P256dh, subscription.Auth);
            var vapidDetails = new VapidDetails("mailto:Stefan.Daniel.Schranz@t-online.de", publicKey, privateKey);
            var webPushClient = new WebPushClient();
            try
            {
                var payload = JsonConvert.SerializeObject(notification);
                await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error sending push notification: " + ex.Message);
            }
        }
    }
}