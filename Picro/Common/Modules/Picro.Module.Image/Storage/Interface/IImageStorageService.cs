using System;
using System.IO;
using System.Threading.Tasks;

namespace Picro.Module.Image.Storage.Interface
{
    public interface IImageStorageService
    {
        Task<bool> UploadImage(Guid userId, Stream imageStream, string fileName);
    }
}