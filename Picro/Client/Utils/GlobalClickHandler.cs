using Microsoft.AspNetCore.Components.Web;

namespace Picro.Client.Utils
{
	public delegate void MouseClickHandler(object sender, MouseEventArgs eventArgs);

	public class GlobalClickHandler
	{
		public event MouseClickHandler OnClick;

		public void HandleClick(MouseEventArgs eventArgs) => OnClick?.Invoke(this, eventArgs);
	}
}