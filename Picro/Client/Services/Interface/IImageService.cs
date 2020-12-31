using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace Picro.Client.Services.Interface
{
    public interface IImageService
    {
        Task UploadImage(IBrowserFile browserFile);
    }
}