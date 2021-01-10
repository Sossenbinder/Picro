using System;
using Picro.Module.User.DataTypes;
using Picro.Module.Image.DataTypes.Response;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Picro.Module.Image.DataTypes.Enums;

namespace Picro.Module.Image.Service.Interface
{
    public interface IImageService
    {
        Task<ImageInfo?> UploadImage(PicroUser user, Stream imageStream);

        Task<IEnumerable<ImageInfo>> GetAllImagesForUser(PicroUser user);

        Task<ImageDeletionErrorCode> DeleteImage(PicroUser user, Guid imageId);
    }
}