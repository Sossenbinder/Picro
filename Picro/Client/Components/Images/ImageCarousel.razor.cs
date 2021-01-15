using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Picro.Client.Components.General.Button;
using Picro.Client.Services.Interface;
using Picro.Common.Extensions;
using Picro.Module.Image.DataTypes.Notification;

namespace Picro.Client.Components.Images
{
	public partial class ImageCarousel : ComponentBase
	{
		[Inject]
		public ISharedImageService SharedImageService { get; set; } = null!;

		[Parameter]
		public Action<ImageShareInfo?> SetCurrentImage { get; set; } = null!;

		private LinkedListNode<ImageShareInfo>? _currentImage = null;

		private LinkedListNode<ImageShareInfo>? CurrentImage
		{
			get => _currentImage;
			set
			{
				_currentImage = value;
				SetCurrentImage(value?.Value);
			}
		}

		private LinkedList<ImageShareInfo> _images;

		private NavigationDirection _lastDirection = NavigationDirection.Right;

		private bool _slideRunning = false;

		protected override async Task OnInitializedAsync()
		{
			await SharedImageService.InitializeSharedImages();

			if (!SharedImageService.SharedImages.IsNullOrEmpty())
			{
				_images = new LinkedList<ImageShareInfo>(SharedImageService.SharedImages);
				CurrentImage = _images.First;
			}
		}

		private void OnNavigationClick(NavigationDirection direction)
		{
			if (_currentImage == null)
			{
				return;
			}

			_slideRunning = true;
			_lastDirection = direction;

			_ = RestoreDelayed(direction);

			StateHasChanged();
		}

		private async Task RestoreDelayed(NavigationDirection direction)
		{
			await Task.Delay(500);

			CurrentImage = direction == NavigationDirection.Left
				? CurrentImage.CircularPrev(_images)
				: CurrentImage.CircularNext(_images);

			_slideRunning = false;

			StateHasChanged();
		}
	}
}