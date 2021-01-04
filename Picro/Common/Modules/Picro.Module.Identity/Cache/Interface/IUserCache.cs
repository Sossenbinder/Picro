using Picro.Module.Identity.DataTypes;
using System;

namespace Picro.Module.Identity.Cache.Interface
{
    public interface IUserCache
    {
        void PutUser(User user);

        User? GetUser(Guid userId);
    }
}