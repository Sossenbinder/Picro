using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Picro.Common.Storage.Extensions;
using Picro.Common.Storage.Utils;
using Picro.Common.Utils.Async;
using Picro.Module.Identity.DataTypes;
using Picro.Module.Image.DataTypes.Entity;
using Picro.Module.Image.Storage.Interface;

namespace Picro.Module.Image.Storage
{
    public class ImageUserMappingTableService : IImageUserMappingTableService
    {
        private readonly AsyncLazy<CloudTable> _imageUserMappingTable;

        public ImageUserMappingTableService(CloudTableClient tableClient)
        {
            _imageUserMappingTable = new AsyncLazy<CloudTable>(() => tableClient.GetExistingTableReference("ImageUserMapping"));
        }

        public async Task<bool> AddNewImageEntryForUser(User user, Guid imageIdentifier, string imageUri)
        {
            var table = await _imageUserMappingTable;

            var imageIdentifierStringified = imageIdentifier.ToString();
            var userImageMappingEntity = new UserImageMappingEntity()
            {
                PartitionKey = imageIdentifierStringified,
                RowKey = imageIdentifierStringified,
                ImageUri = imageUri,
                ImageIdentifier = imageIdentifier,
                UserIdentifier = user.Identifier,
            };

            var operation = TableOperation.InsertOrMerge(userImageMappingEntity);

            var result = await table.ExecuteAsync(operation);

            return result.HasSuccessfulStatusCode();
        }
    }
}