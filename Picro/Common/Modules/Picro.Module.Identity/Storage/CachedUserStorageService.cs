using System;
using System.Threading.Tasks;
using Picro.Module.Identity.Cache.Interface;
using Picro.Module.Identity.DataTypes;
using Picro.Module.Identity.Storage.Interface;

namespace Picro.Module.Identity.Storage
{
    internal class CachedUserStorageService : IUserStorageService
    {
        private readonly ICommonUserStorageService _userStorageService;

        private readonly IUserCache _userCache;

        public CachedUserStorageService(
            IUserCache userCache,
            ICommonUserStorageService userStorageService)
        {
            _userStorageService = userStorageService;
            _userCache = userCache;
        }

        public async Task<bool> InsertUser(User user)
        {
            var innerSuccess = await _userStorageService.InsertUser(user);

            if (innerSuccess)
            {
                _userCache.PutUser(user.Identifier, user!);
            }

            return innerSuccess;
        }

        public async ValueTask<User?> FindUser(Guid clientId)
        {
            var cachedUser = _userCache.GetUser(clientId);

            if (cachedUser != null)
            {
                return cachedUser;
            }

            var user = await _userStorageService.FindUser(clientId);

            if (user != null)
            {
                _userCache.PutUser(clientId, user!);
            }

            return user;
        }

        public async Task<bool> UpdateUser(User user, Action<User> updateAction)
        {
            var innerSuccess = await _userStorageService.UpdateUser(user, updateAction);

            // User is already updated here since inner executes the update action
            if (innerSuccess)
            {
                _userCache.PutUser(user.Identifier, user);
            }

            return innerSuccess;
        }

        public async Task<bool> UpdateUser(Guid userId, Action<User> updateAction)
        {
            var user = await FindUser(userId);

            if (user == null)
            {
                return false;
            }

            return await UpdateUser(user, updateAction);
        }
    }
}