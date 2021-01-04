using Picro.Module.Identity.DataTypes;
using Picro.Module.Image.DataTypes;
using Picro.Module.Image.DataTypes.Response;
using Picro.Module.Image.Event.Interface;
using Picro.Module.Image.Service.Interface;
using Picro.Module.Image.Storage.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Picro.Module.Image.DataTypes.Enums;

namespace Picro.Module.Image.Service
{
    public class ImageService : IImageService
    {
        private readonly IImageStorageService _imageStorageService;

        private readonly IImageUserMappingSqlService _imageUserMappingSqlService;

        private readonly IImageEventHub _imageEventHub;

        public ImageService(
            IImageStorageService imageStorageService,
            IImageUserMappingSqlService imageUserMappingSqlService,
            IImageEventHub imageEventHub)
        {
            _imageStorageService = imageStorageService;
            _imageUserMappingSqlService = imageUserMappingSqlService;
            _imageEventHub = imageEventHub;
        }

        public async Task<ImageInfo?> UploadImage(User user, Stream imageStream)
        {
            var uploadTimestamp = DateTime.UtcNow;
            var imageIdentifier = Guid.NewGuid();
            var fileName = $"{user.Identifier}/{imageIdentifier}.png";

            var (success, imageUri) = await _imageStorageService.UploadImage(user.Identifier, imageStream, fileName);

            if (!success)
            {
                return null;
            }

            var mappingEntrySuccess = await _imageUserMappingSqlService.AddNewImageEntryForUser(user, imageIdentifier, imageUri!, uploadTimestamp);

            if (!mappingEntrySuccess)
            {
                return null;
            }

            // Upload worked fine, now let's share the image around
            _imageEventHub.ImageUploaded.RaiseFireAndForget(new ImageUploadedEvent(user, imageUri!, imageIdentifier));

            return new ImageInfo(imageIdentifier, imageUri!, uploadTimestamp);
        }

        public Task<IEnumerable<ImageInfo>> GetAllImagesForUser(User user) =>
            _imageUserMappingSqlService.GetAllImagesForUser(user);

        public async Task<ImageDeletionErrorCode> DeleteImage(User user, Guid imageId)
        {
            var doesImageBelongToUser = await _imageUserMappingSqlService.DoesImageBelongToUser(user, imageId);

            if (!doesImageBelongToUser)
            {
                return ImageDeletionErrorCode.InvalidAccount;
            }

            var fileName = $"{user.Identifier}/{imageId}.png";

            if (await _imageStorageService.RemoveImage(fileName))
            {
                await _imageUserMappingSqlService.RemoveMapping(user, imageId);
                return ImageDeletionErrorCode.Success;
            }
            else
            {
                return ImageDeletionErrorCode.UnspecifiedError;
            }
        }
    }
}