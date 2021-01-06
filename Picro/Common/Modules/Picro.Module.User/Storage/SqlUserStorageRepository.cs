using Microsoft.EntityFrameworkCore;
using Picro.Common.Storage.Extensions;
using Picro.Module.User.DataTypes;
using Picro.Module.User.Storage.Context;
using Picro.Module.User.Storage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Picro.Module.User.Storage
{
	public class SqlUserStorageRepository : IUserRepository
	{
		private readonly IDbContextFactory<UserContext> _contextFactory;

		public SqlUserStorageRepository(IDbContextFactory<UserContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task<bool> InsertUser(User user)
		{
			await using var ctx = _contextFactory.CreateDbContext();

			ctx.Users.Add(user.ToEntity());

			return await ctx.SaveChangesAsyncWithSuccessResponse();
		}

		public async Task<User?> FindUser(Guid clientId)
		{
			await using var ctx = _contextFactory.CreateDbContext();

			var user = await ctx.Users
				.Where(x => x.UserId == clientId)
				.FirstOrDefaultAsync();

			return user?.ToUserModel();
		}

		public async Task<bool> UpdateUser(User user)
		{
			await using var ctx = _contextFactory.CreateDbContext();

			ctx.Users.Update(user.ToEntity());

			return await ctx.SaveChangesAsyncWithSuccessResponse();
		}

		public async Task<IEnumerable<User>> GetRandomUsers(Guid userIdToExcept, int amount = 5)
		{
			await using var ctx = _contextFactory.CreateDbContext();

			var users = await ctx.Users
				.Where(x => x.UserId != userIdToExcept)
				.OrderBy(x => Guid.NewGuid())
				.Take(amount)
				.ToListAsync();

			return users.Select(x => x.ToUserModel());
		}
	}
}