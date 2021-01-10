using Picro.Module.User.DataTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picro.Module.User.Service.Interface
{
    public interface IUserService
    {
        Task RegisterNewUser(Guid userId);

        Task<bool> IdentifyUser(Guid userId);

        Task<PicroUser?> GetUser(Guid userId);

        /// <summary>
        /// Will not throw but rather swallow all exceptions, so this can properly represent a non-critical unawaitable async operation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastAccessedAtUtc"></param>
        Task UpdateLastAccessedAt(Guid userId, DateTime? lastAccessedAtUtc = null);

        Task<bool> UpdateUser(PicroUser user);

        Task<bool> UpdateUser(Guid userId, Action<PicroUser> updateAction);

        Task<IEnumerable<PicroUser>> GetRandomUsers(Guid idToExclude);

        Task<bool> UpdateNotificationSubscription(PicroUser user, NotificationSubscription subscription);

        Task<NotificationSubscription?> GetNotificationSubscription(PicroUser user);
    }
}