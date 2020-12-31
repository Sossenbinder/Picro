using System;
using System.IO;
using System.Threading.Tasks;
using Picro.Common.Eventing.Events.MassTransit.Interface;
using Picro.Common.Eventing.Helper;
using Picro.Common.Eventing.Notifications;
using Picro.Module.Identity.DataTypes;
using Picro.Module.Image.DataTypes;
using Picro.Module.Image.Event.Interface;
using Picro.Module.Image.Service.Interface;
using Picro.Module.Image.Storage.Interface;

namespace Picro.Module.Image.Service
{
    public class ImageService : IImageService
    {
        private readonly IImageStorageService _imageStorageService;

        private readonly IMassTransitSignalRBackplaneService _massTransitSignalRBackplaneService;

        private readonly IImageEventHub _imageEventHub;

        public ImageService(
            IImageStorageService imageStorageService,
            IMassTransitSignalRBackplaneService massTransitSignalRBackplaneService,
            IImageEventHub imageEventHub)
        {
            _imageStorageService = imageStorageService;
            _massTransitSignalRBackplaneService = massTransitSignalRBackplaneService;
            _imageEventHub = imageEventHub;
        }

        public async Task<bool> UploadImage(User user, Stream imageStream)
        {
            var fileName = $"{user.Identifier}/{Guid.NewGuid()}.png";

            var uploadSuccess = await _imageStorageService.UploadImage(user.Identifier, imageStream, fileName);

            _imageEventHub.ImageUploaded.RaiseFireAndForget(new ImageUploadedEvent("Bla"));

            if (uploadSuccess)
            {
            }

            return uploadSuccess;
        }
    }
}