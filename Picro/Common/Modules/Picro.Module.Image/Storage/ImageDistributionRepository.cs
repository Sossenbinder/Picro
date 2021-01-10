using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Picro.Module.Image.DataTypes;
using Picro.Module.Image.DataTypes.Entity;
using Picro.Module.Image.Storage.Context;
using Picro.Module.Image.Storage.Interface;
using Picro.Module.User.DataTypes;

namespace Picro.Module.Image.Storage
{
    public class ImageDistributionRepository : IImageDistributionRepository
    {
        private readonly IDbContextFactory<ImageDbContext> _contextFactory;

        public ImageDistributionRepository(IDbContextFactory<ImageDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task InsertMapping(Guid imageId, PicroUser user)
        {
            await using var ctx = _contextFactory.CreateDbContext();

            var entity = new ImageDistributionMappingEntity()
            {
                UserId = user.Identifier,
                ImageId = imageId,
                Timestamp = DateTime.UtcNow,
                Acknowledged = false,
            };

            ctx.ImageDistributionMappings.Add(entity);

            await ctx.SaveChangesAsync();
        }

        public async Task InsertMappings(Guid imageId, IEnumerable<PicroUser> users)
        {
            await using var ctx = _contextFactory.CreateDbContext();

            ctx.ImageDistributionMappings.AddRange(users
                .Select(user => new ImageDistributionMappingEntity()
                {
                    UserId = user.Identifier,
                    ImageId = imageId,
                    Timestamp = DateTime.UtcNow,
                    Acknowledged = false,
                }));

            await ctx.SaveChangesAsync();
        }

        public async Task AcknowledgeReceival(Guid imageId, PicroUser user)
        {
            await using var ctx = _contextFactory.CreateDbContext();

            var entity = new ImageDistributionMappingEntity()
            {
                ImageId = imageId,
                UserId = user.Identifier,
                Acknowledged = true,
            };

            ctx.ImageDistributionMappings.Update(entity);

            await ctx.SaveChangesAsync();
        }

        public async Task<IEnumerable<ImageDistributionMapping>> GetImageMappings(Guid imageId)
        {
            await using var ctx = _contextFactory.CreateDbContext();

            var recipients = await ctx.ImageDistributionMappings
                .Where(x => x.ImageId == imageId)
                .Include(x => x.User)
                .ThenInclude(x => x.NotificationSubscription)
                .ToListAsync();

            return recipients.Select(x => x.ToModel());
        }
    }
}