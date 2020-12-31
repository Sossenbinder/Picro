using Picro.Common.Eventing.Notifications;

namespace Picro.Common.Eventing.Helper
{
    public static class FrontendNotificationFactory
    {
        public static FrontendNotification<T> Create<T>(T payload, NotificationType notificationType)
            => CreateNotification(payload, notificationType, Operation.Create);

        public static FrontendNotification<T> Update<T>(T payload, NotificationType notificationType)
            => CreateNotification(payload, notificationType, Operation.Update);

        public static FrontendNotification<T> Delete<T>(T payload, NotificationType notificationType)
            => CreateNotification(payload, notificationType, Operation.Delete);

        private static FrontendNotification<T> CreateNotification<T>(
            T payload,
            NotificationType notificationType,
            Operation operation)
        {
            return new FrontendNotification<T>()
            {
                Data = payload,
                NotificationType = notificationType,
                Operation = operation
            };
        }
    }
}