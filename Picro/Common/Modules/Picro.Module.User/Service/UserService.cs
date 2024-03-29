﻿using Microsoft.Extensions.Logging;
using Picro.Common.Extensions;
using Picro.Module.User.Cache.Interface;
using Picro.Module.User.DataTypes;
using Picro.Module.User.Service.Interface;
using Picro.Module.User.Storage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Picro.Module.User.Service
{
    public class UserService : IUserService
    {
        private readonly IUserCache _userCache;

        private readonly IUserRepository _userRepository;

        private readonly INotificationSubscriptionRepository _notificationSubscriptionRepository;

        private readonly ILogger _logger;

        public UserService(
            IUserCache userCache,
            IUserRepository userRepository,
            ILogger<UserService> logger,
            INotificationSubscriptionRepository notificationSubscriptionRepository)
        {
            _userCache = userCache;
            _userRepository = userRepository;
            _logger = logger;
            _notificationSubscriptionRepository = notificationSubscriptionRepository;
        }

        public async Task RegisterNewUser(Guid userId)
        {
            var user = new PicroUser()
            {
                Identifier = userId,
                LastAccessedAtUtc = DateTime.UtcNow,
            };

            if (await _userRepository.InsertUser(user))
            {
                _userCache.PutUser(user);
            }
        }

        public async Task<bool> IdentifyUser(Guid userId)
        {
            var user = await GetUser(userId);

            if (user == null)
            {
                return false;
            }

#pragma warning disable 4014
            UpdateLastAccessedAt(user, DateTime.UtcNow);
#pragma warning restore 4014

            return true;
        }

        public async Task UpdateLastAccessedAt(Guid userId, DateTime? lastAccessedAtUtc = null)
        {
            var user = await GetUser(userId);
            await UpdateLastAccessedAt(user!, lastAccessedAtUtc);
        }

        public async Task<PicroUser?> GetUser(Guid userId)
        {
            var user = _userCache.GetUser(userId);

            if (user != null)
            {
                return user;
            }

            user = await _userRepository.FindUser(userId);

            if (user != null)
            {
                _userCache.PutUser(user);
            }
            else
            {
                return null;
            }

            return user;
        }

        public async Task<bool> UpdateUser(Guid userId, Action<PicroUser> updateAction)
        {
            var user = await GetUser(userId);

            if (user == null)
            {
                return false;
            }

            updateAction(user);

            var remoteSuccess = await _userRepository.UpdateUser(user);

            if (remoteSuccess)
            {
                _userCache.PutUser(user);
            }

            return remoteSuccess;
        }

        public async Task<bool> UpdateUser(PicroUser user)
        {
            var remoteSuccess = await _userRepository.UpdateUser(user);

            if (remoteSuccess)
            {
                _userCache.PutUser(user);
            }

            return remoteSuccess;
        }

        public async Task<IEnumerable<PicroUser>> GetRandomUsers(Guid idToExclude)
        {
            var randomUsers = (await _userRepository.GetRandomUsers(idToExclude)).ToList();

            return randomUsers;
        }

        public async Task<bool> UpdateNotificationSubscription(PicroUser user, NotificationSubscription subscription)
        {
            var remoteSuccess = await _notificationSubscriptionRepository.InsertNotificationSubscription(user, subscription);

            if (!remoteSuccess)
            {
                return remoteSuccess;
            }

            user.ConnectionInformation ??= new ConnectionInformation();
            user.ConnectionInformation.NotificationSubscription = subscription;

            _userCache.PutUser(user);

            return remoteSuccess;
        }

        public async Task<NotificationSubscription?> GetNotificationSubscription(PicroUser user)
        {
            var subscription = await _notificationSubscriptionRepository.GetNotificationSubscription(user);

            if (subscription == null)
            {
                return subscription;
            }

            user.ConnectionInformation ??= new ConnectionInformation();
            user.ConnectionInformation.NotificationSubscription = subscription;

            _userCache.PutUser(user);

            return subscription;
        }

        private async Task UpdateLastAccessedAt(PicroUser user, DateTime? lastAccessedAtUtc = null)
        {
            try
            {
                var accessTime = lastAccessedAtUtc ?? DateTime.UtcNow;
                user.LastAccessedAtUtc = accessTime;
                await _userRepository.UpdateUser(user);
            }
            catch (Exception e)
            {
                _logger.LogException(e, $"Failed updating last access timestamp for user {user.Identifier}");
            }
        }
    }
}