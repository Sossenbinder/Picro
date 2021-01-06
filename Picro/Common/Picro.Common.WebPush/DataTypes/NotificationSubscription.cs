namespace Picro.Common.WebPush.DataTypes
{
    public class NotificationSubscription
    {
        public string Url { get; set; }

        public string P256dh { get; set; }

        public string Auth { get; set; }
    }
}