using Picro.Module.Identity.DataTypes;
using Picro.Module.Image.DataTypes.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picro.Module.Image.Storage.Interface
{
    public interface IImageUserMappingSqlService
    {
        Task<bool> AddNewImageEntryForUser(User user, Guid imageIdentifier, string imageUri, DateTime uploadTimeStamp);

        Task<IEnumerable<ImageInfo>> GetAllImagesForUser(User user);

        Task<bool> DoesImageBelongToUser(User user, Guid imageIdentifier);

        Task RemoveMapping(User user, Guid imageIdentifier);
    }
}