using System.Threading.Tasks;
using Picro.Module.User.DataTypes;
using Picro.Module.Notification.DataTypes;

namespace Picro.Module.Notification.Storage.Interface
{
	public interface INotificationSubscriptionRepository
	{
		Task<bool> InsertNotificationSubscription(User user, NotificationSubscription subscription);

		Task<NotificationSubscription?> GetNotificationSubscription(User user);
	}
}