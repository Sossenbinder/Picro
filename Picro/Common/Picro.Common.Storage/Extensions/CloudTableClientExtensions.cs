using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace Picro.Common.Storage.Extensions
{
    public static class CloudTableClientExtensions
    {
        public static async Task<CloudTable> GetExistingTableReference(this CloudTableClient client, string tableName)
        {
            var table = client.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }
    }
}