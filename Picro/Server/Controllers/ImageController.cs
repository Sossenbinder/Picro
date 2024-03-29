﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Picro.Module.Image.DataTypes.Enums;
using Picro.Module.Image.DataTypes.Response;
using Picro.Module.Image.Service.Interface;
using Picro.Module.Image.Utils;
using Picro.Module.User.Service.Interface;
using Picro.Server.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Picro.Module.Image.DataTypes.Notification;

namespace Picro.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ImageController : SessionBaseController
	{
		private readonly IImageService _imageService;

		private readonly IImageDistributionService _imageDistributionService;

		public ImageController(
			IImageService imageService,
			IUserService userService,
			IImageDistributionService imageDistributionService)
			: base(userService)
		{
			_imageService = imageService;
			_imageDistributionService = imageDistributionService;
		}

		[HttpPost("Upload")]
		[Authorize]
		public async Task<JsonResponse<ImageUploadInfoResponse>> Upload(IFormFile? file)
		{
			if (file == null)
			{
				return JsonResponse.Error(new ImageUploadInfoResponse(ImageUploadErrorCode.NoFile, null));
			}

			var fileName = GetFileNameExtensions(file.FileName);

			if (!AllowedImageExtensions.ImageExtensions.Contains(fileName))
			{
				return JsonResponse<ImageUploadInfoResponse>.Error(new ImageUploadInfoResponse(ImageUploadErrorCode.InvalidFileEnding, null));
			}

			await using var fileStream = file.OpenReadStream();
			var imageInfo = await _imageService.UploadImage(User, fileStream);

			return imageInfo != null
				? JsonResponse.Success(new ImageUploadInfoResponse(ImageUploadErrorCode.Success, imageInfo))
				: JsonResponse.Error(new ImageUploadInfoResponse(ImageUploadErrorCode.UploadFailed, null)); ;
		}

		[HttpGet("GetUploadedImages")]
		[Authorize]
		public async Task<JsonResponse<IEnumerable<ImageInfo>>> GetUploadedImages()
		{
			var imageInfos = await _imageService.GetAllImagesForUser(User);

			return JsonResponse<IEnumerable<ImageInfo>>.Success(imageInfos);
		}

		[HttpGet("GetReceivedImages")]
		[Authorize]
		public async Task<JsonResponse<IEnumerable<ImageShareInfo>>> GetReceivedImages()
		{
			var imageInfos = await _imageDistributionService.GetImagesSharedToUser(User);

			return JsonResponse<IEnumerable<ImageShareInfo>>.Success(imageInfos);
		}

		[HttpDelete("DeleteImage")]
		[Authorize]
		public async Task<JsonResponse<ImageDeletionErrorCode>> DeleteImage(Guid imageId)
		{
			var result = await _imageService.DeleteImage(User, imageId);

			return result == ImageDeletionErrorCode.Success ? JsonResponse.Success(ImageDeletionErrorCode.Success) : JsonResponse.Error(result);
		}

		private static string GetFileNameExtensions(string fileName) => fileName.Substring(fileName.LastIndexOf('.'));
	}
}