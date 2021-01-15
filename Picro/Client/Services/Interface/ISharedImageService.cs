using System.Collections.Generic;
using System.Threading.Tasks;
using Picro.Common.Eventing.Events;
using Picro.Module.Image.DataTypes.Notification;

namespace Picro.Client.Services.Interface
{
	public interface ISharedImageService
	{
		List<ImageShareInfo> SharedImages { get; }

		AsyncEvent ImageReceived { get; }

		Task InitializeSharedImages();
	}
}