using System.Threading.Tasks;
using Picro.Common.Eventing.Notifications;
using Picro.Module.User.DataTypes;

namespace Picro.Module.Notification.Service.Interface
{
    public interface INotificationService
    {
        Task<bool> RegisterUserForNotifications(PicroUser user, NotificationSubscription subscription);

        Task SendNotificationToSession<T>(PicroUser user, FrontendNotification<T> notification);
    }
}