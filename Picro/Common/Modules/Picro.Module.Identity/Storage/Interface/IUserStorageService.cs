using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Picro.Module.Identity.DataTypes;

namespace Picro.Module.Identity.Storage.Interface
{
    public interface ICommonUserStorageService
    {
        Task<bool> InsertUser(User user);

        ValueTask<User?> FindUser(Guid clientId);

        Task<bool> UpdateUser(User user, Action<User> updateAction);
    }

    /// <summary>
    /// Decorating service which handles user management local and remote
    /// </summary>
    public interface IUserStorageService : ICommonUserStorageService
    {
        Task<bool> UpdateUser(Guid userId, Action<User> updateAction);
    }

    /// <summary>
    /// Special functionality which can't be done locally due to restrictions (E.g. can't grab a random set of
    /// all users from a local cache)
    /// </summary>
    public interface IUsersStorageRepositoryService
    {
        Task<IEnumerable<User>> GetRandomUsers(Guid userIdToExcept, int amount = 5);
    }
}