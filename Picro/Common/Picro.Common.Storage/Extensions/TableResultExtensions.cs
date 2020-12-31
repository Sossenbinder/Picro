using Microsoft.Azure.Cosmos.Table;

namespace Picro.Common.Storage.Extensions
{
    public static class TableResultExtensions
    {
        public static bool HasSuccessfulStatusCode(this TableResult tableResult) =>
            tableResult.HttpStatusCode >= 200 || tableResult.HttpStatusCode < 300;
    }
}