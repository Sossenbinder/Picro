using System;
using System.Threading.Tasks;
using Picro.Module.Identity.DataTypes;

namespace Picro.Module.Identity.Storage.Interface
{
    public interface IIdentityStorageService
    {
        Task<User?> RegisterUser(Guid clientId);

        Task<User?> FindUser(Guid clientId);
    }
}