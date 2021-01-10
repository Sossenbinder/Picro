using Picro.Module.User.DataTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picro.Module.User.Storage.Interface
{
    public interface IUserRepository
    {
        Task<bool> InsertUser(PicroUser user);

        Task<PicroUser?> FindUser(Guid clientId);

        Task<bool> UpdateUser(PicroUser user);

        Task<IEnumerable<PicroUser>> GetRandomUsers(Guid userIdToExcept, int amount = 5);
    }
}