using System;
using Microsoft.AspNetCore.Components;
using Picro.Module.Image.DataTypes.Notification;

namespace Picro.Client.Pages.Home
{
	public partial class ImageShareCenter : ComponentBase
	{
		private ImageShareInfo? _currentImage = null;

		private void OnCurrentImageShareChanged(ImageShareInfo? shareInfo)
		{
			_currentImage = shareInfo;
			StateHasChanged();
		}
	}
}