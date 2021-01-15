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
    public class UploadedImageInfoRepository : IUploadedImageInfoRepository
    {
        private readonly IDbContextFactory<ImageDbContext> _contextFactory;

        public UploadedImageInfoRepository(IDbContextFactory<ImageDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> AddNewImageEntryForUser(PicroUser user, Guid imageIdentifier, string imageUri, DateTime uploadTimeStamp)
        {
            await using var ctx = _contextFactory.CreateDbContext();

            var entity = new UploadedImageInfoEntity()
            {
                ImageId = imageIdentifier,
                UserId = user.Identifier,
                ImageLink = imageUri,
                UploadTimeStamp = uploadTimeStamp,
            };

            ctx.ImageUserMappings.Add(entity);

            return await ctx.SaveChangesAsyncWithSuccessResponse();
        }

        public async Task<IEnumerable<ImageInfo>> GetAllImagesForUser(PicroUser user)
        {
            await using var ctx = _contextFactory.CreateDbContext();

            var imageMappings = await ctx.ImageUserMappings
                .AsNoTracking()
                .Where(x => x.UserId == user.Identifier)
                .ToListAsync();

            return imageMappings.Select(x => new ImageInfo(x.ImageId, x.ImageLink, x.UploadTimeStamp));
        }

        public async Task<bool> DoesImageBelongToUser(PicroUser user, Guid imageIdentifier)
        {
            await using var ctx = _contextFactory.CreateDbContext();

            var imageFound = await ctx.ImageUserMappings
                .Where(x => x.ImageId == imageIdentifier && x.UserId == user.Identifier)
                .AnyAsync();

            return imageFound;
        }

        public async Task RemoveMapping(PicroUser user, Guid imageIdentifier)
        {
            await using var ctx = _contextFactory.CreateDbContext();

            var entity = new UploadedImageInfoEntity()
            {
                ImageId = imageIdentifier,
                UserId = user.Identifier,
            };

            ctx.ImageUserMappings.Remove(entity);

            await ctx.SaveChangesAsync();
        }
    }
}