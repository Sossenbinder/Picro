using System;
using System.Threading.Tasks;
using Picro.Module.Identity.DataTypes;

namespace Picro.Module.Image.Storage.Interface
{
    public interface IImageUserMappingTableService
    {
        Task<bool> AddNewImageEntryForUser(User user, Guid imageIdentifier, string imageUri);
    }
}