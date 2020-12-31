namespace Picro.Common.Eventing.Notifications
{
    public class FrontendNotification<T>
    {
        public Operation Operation { get; set; }

        public NotificationType NotificationType { get; set; }

        public T Data { get; set; }
    }
}