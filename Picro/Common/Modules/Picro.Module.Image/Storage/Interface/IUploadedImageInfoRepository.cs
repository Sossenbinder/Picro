using Picro.Module.User.DataTypes;
using Picro.Module.Image.DataTypes.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picro.Module.Image.Storage.Interface
{
    public interface IUploadedImageInfoRepository
    {
        Task<bool> AddNewImageEntryForUser(PicroUser user, Guid imageIdentifier, string imageUri, DateTime uploadTimeStamp);

        Task<IEnumerable<ImageInfo>> GetAllImagesForUser(PicroUser user);

        Task<bool> DoesImageBelongToUser(PicroUser user, Guid imageIdentifier);

        Task RemoveMapping(PicroUser user, Guid imageIdentifier);
    }
}