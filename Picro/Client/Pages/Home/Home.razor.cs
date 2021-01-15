using Microsoft.AspNetCore.Components;
using Picro.Module.Image.DataTypes.Notification;

namespace Picro.Client.Pages.Home
{
	public partial class Home : ComponentBase
	{
		private ImageShareInfo? _imageShareInfo = null;

		private void OnImageSelectedChange(ImageShareInfo imageShareInfo) => _imageShareInfo = imageShareInfo;
	}
}