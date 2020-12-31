using System;
using Picro.Module.Identity.DataTypes;

namespace Picro.Module.Identity.Cache.Interface
{
    internal interface IUserCache
    {
        void PutUser(Guid userId, User user);

        User? GetUser(Guid userId);
    }
}