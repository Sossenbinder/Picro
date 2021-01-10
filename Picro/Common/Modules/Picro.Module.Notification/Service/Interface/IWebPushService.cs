using System.Threading.Tasks;
using Picro.Common.Eventing.Notifications;
using Picro.Module.User.DataTypes;

namespace Picro.Module.Notification.Service.Interface
{
    public interface IWebPushService
    {
        Task SendNotificationToSession<T>(NotificationSubscription subscription, FrontendNotification<T> notification);
    }
}