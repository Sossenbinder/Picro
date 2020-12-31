using System;
using System.IO;
using System.Threading.Tasks;
using Picro.Module.Image.DataTypes;

namespace Picro.Module.Image.Storage.Interface
{
    public interface IImageStorageService
    {
        Task<ImageUploadInfo> UploadImage(Guid userId, Stream imageStream, string fileName);
    }
}