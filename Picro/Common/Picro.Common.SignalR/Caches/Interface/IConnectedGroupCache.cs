using System;

namespace Picro.Common.SignalR.Caches.Interface
{
    public interface IConnectedGroupCache
    {
        void Connect(Guid userId);

        void Disconnect(Guid userId);

        bool IsGroupConnected(Guid userId);
    }
}