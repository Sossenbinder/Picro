using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Picro.Server.Utils;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Picro.Module.Identity.Service.Interface;
using Picro.Module.Image.DataTypes.Enums;
using Picro.Module.Image.Service.Interface;
using Picro.Module.Image.Utils;

namespace Picro.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : SessionBaseController
    {
        private readonly IImageService _imageService;

        public ImageController(
            IImageService imageService,
            IUserService userService,
            IConfiguration configuration)
            : base(userService,
                configuration)
        {
            _imageService = imageService;
        }

        [HttpPost("Upload")]
        public async Task<JsonDataResponse<ImageUploadResponse>> Upload(IFormFile? file)
        {
            if (file == null)
            {
                return JsonResponse.Error(ImageUploadResponse.NoFile);
            }

            var fileName = GetFileNameExtensions(file.FileName);

            if (!AllowedImageExtensions.ImageExtensions.Contains(fileName))
            {
                return JsonResponse.Error(ImageUploadResponse.InvalidFileEnding);
            }

            await using var fileStream = file.OpenReadStream();
            var uploadResponse = await _imageService.UploadImage(User, fileStream);

            return uploadResponse
                ? JsonResponse.Success(ImageUploadResponse.Success)
                : JsonResponse.Error(ImageUploadResponse.UploadFailed);
        }

        private static string GetFileNameExtensions(string fileName) => fileName.Substring(fileName.LastIndexOf('.'));
    }
}