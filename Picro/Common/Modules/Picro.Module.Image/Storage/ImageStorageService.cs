using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Picro.Common.Utils;
using Picro.Common.Utils.Async;
using Picro.Module.Image.Storage.Interface;

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

        public async Task<bool> UploadImage(Guid userId, Stream imageStream, string fileName)
        {
            var containerClient = await _blobContainerClient;

            var blobClient = containerClient.GetBlobClient(fileName);

            try
            {
                await blobClient.UploadAsync(imageStream, true);
                return true;
            }
            catch (RequestFailedException e)
            {
                _logger.LogError(e, "Upload of image failed");
                return false;
            }
        }
    }
}