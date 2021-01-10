using Picro.Module.User.DataTypes;
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

        private readonly IUploadedImageInfoRepository _uploadedImageInfoRepository;

        private readonly IImageEventHub _imageEventHub;

        public ImageService(
            IImageStorageService imageStorageService,
            IUploadedImageInfoRepository uploadedImageInfoRepository,
            IImageEventHub imageEventHub)
        {
            _imageStorageService = imageStorageService;
            _uploadedImageInfoRepository = uploadedImageInfoRepository;
            _imageEventHub = imageEventHub;
        }

        public async Task<ImageInfo?> UploadImage(PicroUser user, Stream imageStream)
        {
            var uploadTimestamp = DateTime.UtcNow;
            var imageIdentifier = Guid.NewGuid();
            var fileName = $"{user.Identifier}/{imageIdentifier}.png";

            var (success, imageUri) = await _imageStorageService.UploadImage(user.Identifier, imageStream, fileName);

            if (!success)
            {
                return null;
            }

            var mappingEntrySuccess = await _uploadedImageInfoRepository.AddNewImageEntryForUser(user, imageIdentifier, imageUri!, uploadTimestamp);

            if (!mappingEntrySuccess)
            {
                return null;
            }

            // Upload worked fine, now let's share the image around
            _imageEventHub.ImageUploaded.RaiseFireAndForget(new ImageUploadedEvent(user, imageUri!, imageIdentifier));

            return new ImageInfo(imageIdentifier, imageUri!, uploadTimestamp);
        }

        public Task<IEnumerable<ImageInfo>> GetAllImagesForUser(PicroUser user) =>
            _uploadedImageInfoRepository.GetAllImagesForUser(user);

        public async Task<ImageDeletionErrorCode> DeleteImage(PicroUser user, Guid imageId)
        {
            var doesImageBelongToUser = await _uploadedImageInfoRepository.DoesImageBelongToUser(user, imageId);

            if (!doesImageBelongToUser)
            {
                return ImageDeletionErrorCode.InvalidAccount;
            }

            var fileName = $"{user.Identifier}/{imageId}.png";

            if (await _imageStorageService.RemoveImage(fileName))
            {
                await _uploadedImageInfoRepository.RemoveMapping(user, imageId);
                return ImageDeletionErrorCode.Success;
            }
            else
            {
                return ImageDeletionErrorCode.UnspecifiedError;
            }
        }
    }
}