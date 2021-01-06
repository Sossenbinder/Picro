using Microsoft.EntityFrameworkCore;
using Picro.Common.Storage.Extensions;
using Picro.Module.User.DataTypes;
using Picro.Module.Image.DataTypes.Entity;
using Picro.Module.Image.DataTypes.Response;
using Picro.Module.Image.Storage.Context;
using Picro.Module.Image.Storage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Picro.Module.Image.Storage
{
	public class ImageUserMappingSqlService : IImageUserMappingSqlService
	{
		private readonly IDbContextFactory<ImageContext> _contextFactory;

		public ImageUserMappingSqlService(IDbContextFactory<ImageContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task<bool> AddNewImageEntryForUser(User user, Guid imageIdentifier, string imageUri, DateTime uploadTimeStamp)
		{
			await using var ctx = _contextFactory.CreateDbContext();

			var entity = new ImageUserMappingEntity()
			{
				ImageId = imageIdentifier,
				UserId = user.Identifier,
				ImageLink = imageUri,
				UploadTimeStamp = uploadTimeStamp,
			};

			ctx.ImageUserMapping.Add(entity);

			return await ctx.SaveChangesAsyncWithSuccessResponse();
		}

		public async Task<IEnumerable<ImageInfo>> GetAllImagesForUser(User user)
		{
			await using var ctx = _contextFactory.CreateDbContext();

			var imageMappings = await ctx.ImageUserMapping
				.Where(x => x.UserId == user.Identifier)
				.ToListAsync();

			return imageMappings.Select(x => new ImageInfo(x.ImageId, x.ImageLink, DateTime.UtcNow));
		}

		public async Task<bool> DoesImageBelongToUser(User user, Guid imageIdentifier)
		{
			await using var ctx = _contextFactory.CreateDbContext();

			var imageFound = await ctx.ImageUserMapping
				.Where(x => x.UserId == user.Identifier && x.ImageId == imageIdentifier)
				.AnyAsync();

			return imageFound;
		}

		public async Task RemoveMapping(User user, Guid imageIdentifier)
		{
			await using var ctx = _contextFactory.CreateDbContext();

			var entity = new ImageUserMappingEntity()
			{
				ImageId = imageIdentifier,
				UserId = user.Identifier,
			};

			ctx.ImageUserMapping.Remove(entity);

			await ctx.SaveChangesAsync();
		}
	}
}