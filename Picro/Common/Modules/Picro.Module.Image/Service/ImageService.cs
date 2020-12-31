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

        private readonly IImageUserMappingTableService _imageUserMappingTableService;

        private readonly IImageEventHub _imageEventHub;

        public ImageService(
            IImageStorageService imageStorageService,
            IImageUserMappingTableService imageUserMappingTableService,
            IImageEventHub imageEventHub)
        {
            _imageStorageService = imageStorageService;
            _imageUserMappingTableService = imageUserMappingTableService;
            _imageEventHub = imageEventHub;
        }

        public async Task<bool> UploadImage(User user, Stream imageStream)
        {
            var imageIdentifier = Guid.NewGuid();
            var fileName = $"{user.Identifier}/{imageIdentifier}.png";

            var (success, uri) = await _imageStorageService.UploadImage(user.Identifier, imageStream, fileName);

            if (!success)
            {
                return false;
            }

            var mappingEntrySuccess = await _imageUserMappingTableService.AddNewImageEntryForUser(user, imageIdentifier, uri!);

            if (!mappingEntrySuccess)
            {
                return false;
            }

            // Upload worked fine, now let's share the image around
            _imageEventHub.ImageUploaded.RaiseFireAndForget(new ImageUploadedEvent(user, uri!));

            return true;
        }
    }
}