using System.Threading.Tasks;
using Picro.Module.Identity.Service.Interface;
using Picro.Module.Image.DataTypes;
using Picro.Module.Image.Event.Interface;
using Picro.Module.Image.Service.Interface;

namespace Picro.Module.Image.Service
{
    public class ImageDistributionService : IImageDistributionService
    {
        private readonly IUserService _userService;

        public ImageDistributionService(
            IImageEventHub imageEventHub,
            IUserService userService)
        {
            _userService = userService;

            imageEventHub.ImageUploaded.Register(OnImageUploaded);
        }

        private async Task OnImageUploaded(ImageUploadedEvent args)
        {
        }
    }
}