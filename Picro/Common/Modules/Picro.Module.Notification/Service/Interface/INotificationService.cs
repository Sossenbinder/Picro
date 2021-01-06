using System.Threading.Tasks;
using Picro.Module.User.DataTypes;
using Picro.Module.Notification.DataTypes;

namespace Picro.Module.Notification.Service.Interface
{
	public interface INotificationService
	{
		Task<bool> RegisterUserForNotifications(User user, NotificationSubscription subscription);

		Task SendNotificationToSession(User user);
	}
}