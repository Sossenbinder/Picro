using System.Threading.Tasks;
using Picro.Common.Eventing.Helper;
using Picro.Common.Eventing.Notifications;
using Picro.Module.User.DataTypes;
using Picro.Module.Notification.DataTypes;
using Picro.Module.Notification.Service.Interface;
using Picro.Module.Notification.Storage.Interface;

namespace Picro.Module.Notification.Service
{
	public class NotificationService : INotificationService
	{
		private readonly INotificationSubscriptionRepository _notificationSubscriptionRepository;

		private readonly IWebPushService _webPushService;

		public NotificationService(
			INotificationSubscriptionRepository notificationSubscriptionRepository,
			IWebPushService webPushService)
		{
			_notificationSubscriptionRepository = notificationSubscriptionRepository;
			_webPushService = webPushService;
		}

		public Task<bool> RegisterUserForNotifications(User user, NotificationSubscription subscription)
		{
			return _notificationSubscriptionRepository.InsertNotificationSubscription(user, subscription);
		}

		public async Task SendNotificationToSession(User user)
		{
			var subscription = await _notificationSubscriptionRepository.GetNotificationSubscription(user);

			await _webPushService.SendNotificationToSession(subscription,
				FrontendNotificationFactory.Create<string>("Bla", NotificationType.ImageShared));
		}
	}
}