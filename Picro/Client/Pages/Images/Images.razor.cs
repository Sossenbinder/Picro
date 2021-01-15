using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Picro.Client.Services.Interface;
using Picro.Module.Image.DataTypes.Enums;
using Picro.Module.Image.DataTypes.Response;
using Picro.Module.Image.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;

namespace Picro.Client.Pages.Images
{
	public partial class Images : ComponentBase
	{
		[Inject]
		private IPersonalImageService ImageService { get; set; } = null!;

		private List<ImageInfo> _uploadedImages = new();

		private string _allowedImageExtensions = null!;

		private string selectedSlide = "2";

		protected override async Task OnInitializedAsync()
		{
			_allowedImageExtensions = string.Join(',', AllowedImageExtensions.ImageExtensions);

			_uploadedImages = (await ImageService.GetUploadedImages()).ToList();
		}

		private async Task OnInputFileChange(InputFileChangeEventArgs args)
		{
			var imageInfo = await ImageService.UploadImage(args.File);

			if (imageInfo != null)
			{
				_uploadedImages.Add(imageInfo);

				StateHasChanged();
			}
		}

		private async Task OnDeleteImage(Guid imageId)
		{
			var response = await ImageService.DeleteImage(imageId);

			if (response == ImageDeletionErrorCode.Success)
			{
				_uploadedImages.RemoveAll(x => x.ImageId == imageId);
				StateHasChanged();
			}
		}
	}
}