using System;
using System.Threading.Tasks;
using Picro.Module.Identity.DataTypes;

namespace Picro.Module.Identity.Service.Interface
{
    public interface IUserIdentityService
    {
        Task RegisterNewUser(Guid clientId);

        Task<bool> IdentifyUser(Guid clientId);

        Task<User?> GetUser(Guid clientId);
    }
}