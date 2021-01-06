using System.Threading.Tasks;
using Picro.Common.Eventing.Notifications;
using Picro.Module.Notification.DataTypes;

namespace Picro.Module.Notification.Service.Interface
{
    public interface IWebPushService
    {
        Task SendNotificationToSession(NotificationSubscription subscription, FrontendNotification<string> notification);
    }
}