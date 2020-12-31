using System;
using System.Collections.Concurrent;
using Picro.Module.Identity.Cache.Interface;
using Picro.Module.Identity.DataTypes;

namespace Picro.Module.Identity.Cache
{
    public class ConnectedUserCache : IConnectedUserCache
    {
        private readonly ConcurrentDictionary<Guid, User> _connectedUsers;

        public ConnectedUserCache()
        {
            _connectedUsers = new ConcurrentDictionary<Guid, User>();
        }

        public void InsertUser(Guid userId, User user)
        {
            _connectedUsers[userId] = user;
        }

        public void UpdateUserConnectionId(Guid userId, string? connectionId)
        {
            if (!_connectedUsers.TryGetValue(userId, out var user))
            {
                return;
            }

            lock (user)
            {
                user.ConnectionId = connectionId;
            }
        }

        public User? GetUser(Guid userId)
        {
            return _connectedUsers.TryGetValue(userId, out var user) ? user : null;
        }
    }
}