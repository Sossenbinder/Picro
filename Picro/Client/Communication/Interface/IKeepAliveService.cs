using System;
using System.Threading.Tasks;
using Picro.Common.Eventing.Notifications;

namespace Picro.Client.Communication.Interface
{
    public interface IKeepAliveService
    {
        Task InitializeConnection();

        IDisposable RegisterHandler<T>(NotificationType notificationType, Action<FrontendNotification<T>> handler);

        IDisposable RegisterHandler<T>(NotificationType notificationType, Func<FrontendNotification<T>, Task> handler);

        void RemoveAllHandlers(NotificationType notificationType);
    }
}