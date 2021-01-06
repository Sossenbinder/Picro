using Picro.Module.User.Cache.Interface;
using Picro.Module.User.DataTypes;
using System;
using System.Collections.Concurrent;

namespace Picro.Module.User.Cache
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