using Picro.Module.Identity.DataTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picro.Module.Identity.Storage.Interface
{
    public interface IUserRepository
    {
        Task<bool> InsertUser(User user);

        Task<User?> FindUser(Guid clientId);

        Task<bool> UpdateUser(User user);

        Task<IEnumerable<User>> GetRandomUsers(Guid userIdToExcept, int amount = 5);
    }
}