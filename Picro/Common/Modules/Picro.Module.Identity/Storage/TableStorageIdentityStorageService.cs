using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Picro.Common.Storage.Extensions;
using Picro.Common.Storage.Utils;
using Picro.Common.Utils.Async;
using Picro.Module.Identity.DataTypes;
using Picro.Module.Identity.DataTypes.Entity;
using Picro.Module.Identity.Storage.Interface;

namespace Picro.Module.Identity.Storage
{
    public class TableStorageIdentityStorageService : IIdentityStorageService
    {
        private readonly AsyncLazy<CloudTable> _usersTable;

        public TableStorageIdentityStorageService(CloudTableClient tableClient)
        {
            _usersTable = new AsyncLazy<CloudTable>(() => tableClient.GetExistingTableReference("Users"));
        }

        public async Task<User?> RegisterUser(Guid clientId)
        {
            var table = await _usersTable;

            var user = new UserEntity()
            {
                Identifier = clientId,
                RowKey = clientId.ToString(),
                PartitionKey = TableStoragePartitioner.Partition(clientId)
            };

            var operation = TableOperation.InsertOrMerge(user);

            var result = await table.ExecuteAsync(operation);

            if (result.HttpStatusCode < 200 || result.HttpStatusCode >= 300)
            {
                return null;
            }

            return user.ToUserModel();
        }

        public async Task<User?> FindUser(Guid clientId)
        {
            var table = await _usersTable;

            var partitionKey = TableStoragePartitioner.Partition(clientId);

            var query = new TableQuery<UserEntity>()
                .AndWhereEquals(nameof(UserEntity.PartitionKey), partitionKey)
                .AndWhereEquals(nameof(UserEntity.RowKey), clientId.ToString());

            var result = await table.ExecuteQueryFull(query);

            return result.SingleOrDefault()?.ToUserModel();
        }
    }
}