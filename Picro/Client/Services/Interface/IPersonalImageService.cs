using Microsoft.AspNetCore.Components.Forms;
using Picro.Module.Image.DataTypes.Enums;
using Picro.Module.Image.DataTypes.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picro.Client.Services.Interface
{
	public interface IPersonalImageService
	{
		Task<IEnumerable<ImageInfo>> GetUploadedImages();

		Task<ImageInfo?> UploadImage(IBrowserFile browserFile);

		Task<ImageDeletionErrorCode> DeleteImage(Guid imageId);
	}
}