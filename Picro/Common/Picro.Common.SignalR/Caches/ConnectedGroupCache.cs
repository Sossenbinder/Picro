using Picro.Common.SignalR.Caches.Interface;
using System;
using System.Collections.Generic;

namespace Picro.Common.SignalR.Caches
{
    public class ConnectedGroupCache : IConnectedGroupCache
    {
        private readonly ISet<Guid> _connectedGroups;

        public ConnectedGroupCache()
        {
            _connectedGroups = new HashSet<Guid>();
        }

        public void Connect(Guid userId)
        {
            _connectedGroups.Add(userId);
        }

        public void Disconnect(Guid userId)
        {
            _connectedGroups.Remove(userId);
        }

        public bool IsGroupConnected(Guid userId)
        {
            return _connectedGroups.Contains(userId);
        }
    }
}