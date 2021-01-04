using System;
using Picro.Module.Identity.DataTypes;
using Picro.Module.Image.DataTypes.Response;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Picro.Module.Image.DataTypes.Enums;

namespace Picro.Module.Image.Service.Interface
{
    public interface IImageService
    {
        Task<ImageInfo?> UploadImage(User user, Stream imageStream);

        Task<IEnumerable<ImageInfo>> GetAllImagesForUser(User user);

        Task<ImageDeletionErrorCode> DeleteImage(User user, Guid imageId);
    }
}