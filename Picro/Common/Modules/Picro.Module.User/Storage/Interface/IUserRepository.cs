using Picro.Module.User.DataTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picro.Module.User.Storage.Interface
{
	public interface IUserRepository
	{
		Task<bool> InsertUser(User user);

		Task<User?> FindUser(Guid clientId);

		Task<bool> UpdateUser(User user);

		Task<IEnumerable<User>> GetRandomUsers(Guid userIdToExcept, int amount = 5);
	}
}