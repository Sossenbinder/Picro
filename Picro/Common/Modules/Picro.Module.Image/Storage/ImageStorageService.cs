using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Picro.Common.Utils.Async;
using Picro.Module.Image.DataTypes;
using Picro.Module.Image.Storage.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Picro.Module.Image.Storage
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly AsyncLazy<BlobContainerClient> _blobContainerClient;

        private readonly ILogger _logger;

        public ImageStorageService(
            ILogger<ImageStorageService> logger,
            BlobServiceClient blobServiceClient)
        {
            _logger = logger;

            _blobContainerClient = new AsyncLazy<BlobContainerClient>(async () =>
            {
                var client = blobServiceClient.GetBlobContainerClient("images");

                await client.CreateIfNotExistsAsync();

                return client;
            });
        }

        public async Task<ImageUploadInfo> UploadImage(Guid userId, Stream imageStream, string fileName)
        {
            var containerClient = await _blobContainerClient;

            var blobClient = containerClient.GetBlobClient(fileName);

            try
            {
                await blobClient.UploadAsync(imageStream, true);

                return new ImageUploadInfo(true, blobClient.Uri.ToString());
            }
            catch (RequestFailedException e)
            {
                _logger.LogError(e, "Upload of image failed");
                return new ImageUploadInfo(false, null);
            }
        }

        public async Task<bool> RemoveImage(string fileName)
        {
            var containerClient = await _blobContainerClient;

            var response = await containerClient.DeleteBlobIfExistsAsync(fileName);

            return response.Value;
        }
    }
}