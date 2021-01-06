using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Picro.Module.Notification.Storage.Interface;

namespace Picro.Module.Connection.Storage
{
    public class NotificationSubscriptionRepository : INotificationSubscriptionRepository
    {
        private readonly IDbContextFactory<NotificationDbContext> _contextFactory;

        public NotificationSubscriptionRepository(IDbContextFactory<NotificationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> InsertNotificationSubscription(User user, NotificationSubscription subscription)
        {
            await using var ctx = _contextFactory.CreateDbContext();

            var existingEntity = await ctx.NotificationSubscriptions
                .Where(x => x.UserId == user.Identifier)
                .FirstOrDefaultAsync();

            var entity = subscription.ToEntity();
            entity.UserId = user.Identifier;

            if (existingEntity == null)
            {
                ctx.NotificationSubscriptions.Add(entity);
            }
            else
            {
                existingEntity.Auth = subscription.Auth;
                existingEntity.P256dh = subscription.P256dh;
                existingEntity.Url = subscription.Url;
            }

            return await ctx.SaveChangesAsyncWithSuccessResponse();
        }

        public async Task<NotificationSubscription?> GetNotificationSubscription(User user)
        {
            await using var ctx = _contextFactory.CreateDbContext();

            var notificationSubscriptionEntity = await ctx.NotificationSubscriptions
                .Where(x => x.UserId == user.Identifier)
                .FirstOrDefaultAsync();

            return notificationSubscriptionEntity?.ToModel();
        }
    }
}