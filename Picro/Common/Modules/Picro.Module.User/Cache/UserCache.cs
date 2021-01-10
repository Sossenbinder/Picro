using Picro.Module.User.Cache.Interface;
using Picro.Module.User.DataTypes;
using System;
using System.Collections.Concurrent;

namespace Picro.Module.User.Cache
{
    public class UserCache : IUserCache
    {
        private readonly ConcurrentDictionary<Guid, PicroUser> _users;

        public UserCache()
        {
            _users = new ConcurrentDictionary<Guid, PicroUser>();
        }

        public void PutUser(PicroUser user)
        {
            _users[user.Identifier] = user;
        }

        public PicroUser? GetUser(Guid userId)
        {
            return _users.TryGetValue(userId, out var user) ? user : null;
        }
    }
}