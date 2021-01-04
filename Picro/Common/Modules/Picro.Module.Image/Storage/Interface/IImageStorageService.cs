using Picro.Module.Image.DataTypes;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Picro.Module.Image.Storage.Interface
{
    public interface IImageStorageService
    {
        Task<ImageUploadInfo> UploadImage(Guid userId, Stream imageStream, string fileName);

        Task<bool> RemoveImage(string fileName);
    }
}