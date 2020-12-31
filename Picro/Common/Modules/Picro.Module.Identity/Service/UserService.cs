using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Picro.Common.Extensions;
using Picro.Module.Identity.DataTypes;
using Picro.Module.Identity.Service.Interface;
using Picro.Module.Identity.Storage.Interface;

namespace Picro.Module.Identity.Service
{
    public class UserService : IUserService
    {
        private readonly IUserStorageService _userStorageService;

        private readonly IUsersStorageRepositoryService _usersStorageRepositoryService;

        private readonly ILogger _logger;

        public UserService(
            IUserStorageService userStorageService,
            IUsersStorageRepositoryService usersStorageRepositoryService,
            ILogger<UserService> logger)
        {
            _userStorageService = userStorageService;
            _logger = logger;
            _usersStorageRepositoryService = usersStorageRepositoryService;
        }

        public async Task RegisterNewUser(Guid clientId)
        {
            var user = new User()
            {
                Identifier = clientId,
                LastAccessedAtUtc = DateTime.UtcNow,
            };

            await _userStorageService.InsertUser(user);
        }

        public async Task<bool> IdentifyUser(Guid clientId)
        {
            var user = await _userStorageService.FindUser(clientId);

            if (user == null)
            {
                return false;
            }

#pragma warning disable 4014
            UpdateLastAccessedAt(clientId, DateTime.UtcNow);
#pragma warning restore 4014

            return true;
        }

        public async Task UpdateLastAccessedAt(Guid clientId, DateTime? lastAccessedAtUtc = null)
        {
            try
            {
                var accessTime = lastAccessedAtUtc ?? DateTime.UtcNow;
                await _userStorageService.UpdateUser(clientId, x => x.LastAccessedAtUtc = accessTime);
            }
            catch (Exception e)
            {
                _logger.LogException(e, $"Failed updating last access timestamp for client {clientId}");
            }
        }

        public ValueTask<User?> GetUser(Guid clientId)
            => _userStorageService.FindUser(clientId);

        public Task UpdateUser(Guid clientId, Action<User> updateAction) =>
            _userStorageService.UpdateUser(clientId, updateAction);

        public async Task<IEnumerable<User>> GetRandomUsers(Guid idToExclude)
        {
            return new List<User>() { await GetUser(idToExclude) };
        }
    }
}