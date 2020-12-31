using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Picro.Module.Identity.DataTypes;
using Picro.Module.Identity.Storage.Interface;

namespace Picro.Module.Identity.Storage
{
    public class SqlUserStorageService : ICommonUserStorageService, IUsersStorageRepositoryService
    {
        public Task<bool> InsertUser(User user)
        {
            throw new NotImplementedException();
        }

        public ValueTask<User?> FindUser(Guid clientId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUser(User user, Action<User> updateAction)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetRandomUsers(Guid userIdToExcept, int amount = 5)
        {
            throw new NotImplementedException();
        }
    }
}