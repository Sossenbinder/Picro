using System;
using Microsoft.AspNetCore.Components;

namespace Picro.Client.Components.Images
{
	public partial class ImageReactionHub : ComponentBase
	{
		[Parameter]
		public Guid? ImageId { get; set; }

		protected override void OnParametersSet()
		{
			Console.WriteLine(ImageId);
		}
	}
}