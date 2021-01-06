using Picro.Common.Eventing.Events.MassTransit.Interface;
using Picro.Common.Eventing.Helper;
using Picro.Common.Eventing.Notifications;
using Picro.Common.Extensions.Async;
using Picro.Common.SignalR.Caches.Interface;
using Picro.Module.User.Service.Interface;
using Picro.Module.Image.DataTypes;
using Picro.Module.Image.Event.Interface;
using Picro.Module.Image.Service.Interface;
using System.Linq;
using System.Threading.Tasks;
using Picro.Module.Image.DataTypes.Notification;
using Picro.Module.Image.DataTypes.Response;
using Picro.Module.Notification.Service.Interface;

namespace Picro.Module.Image.Service
{
	public class ImageDistributionService : IImageDistributionService
	{
		private readonly IUserService _userService;

		private readonly IConnectedGroupCache _connectedGroupCache;

		private readonly IMassTransitSignalRBackplaneService _massTransitSignalRBackplaneService;

		private readonly INotificationService _notificationService;

		public ImageDistributionService(
			IImageEventHub imageEventHub,
			IUserService userService,
			IConnectedGroupCache connectedGroupCache,
			IMassTransitSignalRBackplaneService massTransitSignalRBackplaneService,
			INotificationService notificationService)
		{
			_userService = userService;
			_massTransitSignalRBackplaneService = massTransitSignalRBackplaneService;
			_notificationService = notificationService;
			_connectedGroupCache = connectedGroupCache;

			imageEventHub.ImageUploaded.Register(OnImageUploaded);
		}

		private async Task OnImageUploaded(ImageUploadedEvent args)
		{
			var (uploader, imageUri, imageId) = args;

			var recipients = await _userService.GetRandomUsers(uploader.Identifier);

			var imageInfo = new ImageShareInfo(imageId, imageUri);
			var notification = FrontendNotificationFactory.Create(imageInfo, NotificationType.ImageShared);

			await recipients
				.Where(x => _connectedGroupCache.IsGroupConnected(x.Identifier))
				.ParallelAsync(x => _notificationService.SendNotificationToSession(x));

			await recipients
				.Where(x => _connectedGroupCache.IsGroupConnected(x.Identifier))
				.ParallelAsync(x => _massTransitSignalRBackplaneService.RaiseGroupSignalREvent(x.Identifier.ToString(), notification));
		}
	}
}