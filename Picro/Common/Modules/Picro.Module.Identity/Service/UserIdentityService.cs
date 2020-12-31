using System;
using System.Threading.Tasks;
using Picro.Module.Identity.Cache.Interface;
using Picro.Module.Identity.DataTypes;
using Picro.Module.Identity.Service.Interface;
using Picro.Module.Identity.Storage.Interface;

namespace Picro.Module.Identity.Service
{
    public class UserIdentityService : IUserIdentityService
    {
        private readonly IIdentityStorageService _identityStorageService;

        private readonly IConnectedUserCache _connectedUserCache;

        public UserIdentityService(
            IIdentityStorageService identityStorageService,
            IConnectedUserCache connectedUserCache)
        {
            _identityStorageService = identityStorageService;
            _connectedUserCache = connectedUserCache;
        }

        public async Task RegisterNewUser(Guid clientId)
        {
            var user = await _identityStorageService.RegisterUser(clientId);
            _connectedUserCache.InsertUser(clientId, user!);
        }

        public async Task<bool> IdentifyUser(Guid clientId)
        {
            var user = await _identityStorageService.FindUser(clientId);
            _connectedUserCache.InsertUser(clientId, user!);

            return true;
        }

        public async Task<User?> GetUser(Guid clientId)
        {
            var cachedUser = _connectedUserCache.GetUser(clientId);

            if (cachedUser != null)
            {
                return cachedUser;
            }

            return await _identityStorageService.FindUser(clientId);
        }
    }
}