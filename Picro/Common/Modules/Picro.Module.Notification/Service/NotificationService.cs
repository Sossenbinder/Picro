using System;
using System.Threading.Tasks;
using Picro.Common.Eventing.Notifications;
using Picro.Module.User.DataTypes;
using Picro.Module.Notification.Service.Interface;
using Picro.Module.User.Service.Interface;

namespace Picro.Module.Notification.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IUserService _userService;

        private readonly IWebPushService _webPushService;

        public NotificationService(
            IUserService userService,
            IWebPushService webPushService)
        {
            _userService = userService;
            _webPushService = webPushService;
        }

        public Task<bool> RegisterUserForNotifications(PicroUser user, NotificationSubscription subscription)
        {
            return _userService.UpdateNotificationSubscription(user, subscription);
        }

        public async Task SendNotificationToSession<T>(PicroUser user, FrontendNotification<T> notification)
        {
            var subscription = user.ConnectionInformation?.NotificationSubscription;

            if (subscription == null)
            {
                throw new ArgumentException("User doesn't have a subscription");
            }

            await _webPushService.SendNotificationToSession(subscription, notification);
        }
    }
}