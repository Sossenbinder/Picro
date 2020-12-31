using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Picro.Common.Storage.Extensions;
using Picro.Common.Utils.Async;
using Picro.Module.Identity.DataTypes;
using Picro.Module.Identity.DataTypes.Entity;
using Picro.Module.Identity.Storage.Interface;

namespace Picro.Module.Identity.Storage
{
    public class TableStorageUserStorageService : ICommonUserStorageService, IUsersStorageRepositoryService
    {
        private readonly AsyncLazy<CloudTable> _usersTable;

        public TableStorageUserStorageService(CloudTableClient tableClient)
        {
            _usersTable = new AsyncLazy<CloudTable>(() => tableClient.GetExistingTableReference("Users"));
        }

        public async Task<bool> InsertUser(User user)
        {
            var table = await _usersTable;

            var userEntity = user.ToEntity();

            var operation = TableOperation.Insert(userEntity);

            var result = await table.ExecuteAsync(operation);

            return result.HasSuccessfulStatusCode();
        }

        public async ValueTask<User?> FindUser(Guid clientId)
        {
            var table = await _usersTable;

            var clientIdStringified = clientId.ToString();

            var query = new TableQuery<UserEntity>()
                .AndWhereEquals(nameof(UserEntity.PartitionKey), clientIdStringified)
                .AndWhereEquals(nameof(UserEntity.RowKey), clientIdStringified);

            var result = await table.ExecuteQueryFull(query);

            return result.SingleOrDefault()?.ToUserModel();
        }

        public async Task<bool> UpdateUser(User user, Action<User> updateAction)
        {
            var table = await _usersTable;

            updateAction(user);

            var userEntity = user.ToEntity();

            var operation = TableOperation.Merge(userEntity);

            var tableResult = await table.ExecuteAsync(operation);

            return tableResult.HasSuccessfulStatusCode();
        }

        public async Task<IEnumerable<User>> GetRandomUsers(Guid userIdToExcept, int amount = 5)
        {
            throw new NotImplementedException();
        }
    }
}