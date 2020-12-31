using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Picro.Module.Identity.DataTypes;

namespace Picro.Module.Identity.Service.Interface
{
    public interface IUserService
    {
        Task RegisterNewUser(Guid clientId);

        Task<bool> IdentifyUser(Guid clientId);

        ValueTask<User?> GetUser(Guid clientId);

        /// <summary>
        /// Will not throw but rather swallow all exceptions, so this can properly represent a non-critical unawaitable async operation
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="lastAccessedAtUtc"></param>
        Task UpdateLastAccessedAt(Guid clientId, DateTime? lastAccessedAtUtc = null);

        Task UpdateUser(Guid clientId, Action<User> updateAction);

        Task<IEnumerable<User>> GetRandomUsers(Guid idToExclude);
    }
}