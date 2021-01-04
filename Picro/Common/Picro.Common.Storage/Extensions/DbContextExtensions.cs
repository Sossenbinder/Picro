using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Picro.Common.Storage.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task<bool> SaveChangesAsyncWithSuccessResponse(this DbContext dbContext)
        {
            var affectedRows = await dbContext.SaveChangesAsync();

            return affectedRows > 0;
        }
    }
}