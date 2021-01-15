using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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