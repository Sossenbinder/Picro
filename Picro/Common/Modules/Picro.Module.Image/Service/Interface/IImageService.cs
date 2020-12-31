using System.IO;
using System.Threading.Tasks;
using Picro.Module.Identity.DataTypes;

namespace Picro.Module.Image.Service.Interface
{
    public interface IImageService
    {
        Task<bool> UploadImage(User user, Stream imageStream);
    }
}