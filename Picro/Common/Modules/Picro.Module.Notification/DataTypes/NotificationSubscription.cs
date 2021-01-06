using Picro.Module.Notification.DataTypes.Entity;

namespace Picro.Module.Notification.DataTypes
{
    public class NotificationSubscription
    {
        public string Url { get; set; }

        public string P256dh { get; set; }

        public string Auth { get; set; }

        public NotificationSubscriptionEntity ToEntity()
        {
            return new()
            {
                Auth = Auth,
                Url = Url,
                P256dh = P256dh,
            };
        }
    }
}