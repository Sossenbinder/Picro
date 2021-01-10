using Picro.Module.User.DataTypes;
using System;

namespace Picro.Module.User.Cache.Interface
{
    public interface IUserCache
    {
        void PutUser(PicroUser user);

        PicroUser? GetUser(Guid userId);
    }
}