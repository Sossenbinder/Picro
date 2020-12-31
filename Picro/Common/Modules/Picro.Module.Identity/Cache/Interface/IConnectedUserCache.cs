using System;
using Picro.Module.Identity.DataTypes;

namespace Picro.Module.Identity.Cache.Interface
{
    public interface IConnectedUserCache
    {
        void InsertUser(Guid userId, User user);

        void UpdateUserConnectionId(Guid userId, string? connectionId);

        User? GetUser(Guid userId);
    }
}