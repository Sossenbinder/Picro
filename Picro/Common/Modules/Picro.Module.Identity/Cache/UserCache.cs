using Picro.Module.Identity.Cache.Interface;
using Picro.Module.Identity.DataTypes;
using System;
using System.Collections.Concurrent;

namespace Picro.Module.Identity.Cache
{
    public class UserCache : IUserCache
    {
        private readonly ConcurrentDictionary<Guid, User> _users;

        public UserCache()
        {
            _users = new ConcurrentDictionary<Guid, User>();
        }

        public void PutUser(User user)
        {
            _users[user.Identifier] = user;
        }

        public User? GetUser(Guid userId)
        {
            return _users.TryGetValue(userId, out var user) ? user : null;
        }
    }
}