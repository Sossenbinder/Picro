using System;
using Microsoft.AspNetCore.Components.Forms;
using Picro.Module.Image.DataTypes.Response;
using System.Collections.Generic;
using System.Threading.Tasks;
using Picro.Module.Image.DataTypes.Enums;

namespace Picro.Client.Services.Interface
{
    public interface IPersonalImageService
    {
        Task<IEnumerable<ImageInfo>> GetImages();

        Task<ImageInfo?> UploadImage(IBrowserFile browserFile);

        Task<ImageDeletionErrorCode> DeleteImage(Guid imageId);
    }
}