using System;
using System.Collections.Generic;
using Picro.Common.Eventing.Events.MassTransit.Interface;
using Picro.Common.Eventing.Helper;
using Picro.Common.Eventing.Notifications;
using Picro.Common.Extensions.Async;
using Picro.Module.User.Service.Interface;
using Picro.Module.Image.DataTypes;
using Picro.Module.Image.Event.Interface;
using Picro.Module.Image.Service.Interface;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Picro.Common.Extensions;
using Picro.Module.Image.DataTypes.Notification;
using Picro.Module.Image.Storage.Interface;
using Picro.Module.Notification.Service.Interface;
using Picro.Module.User.DataTypes;

namespace Picro.Module.Image.Service
{
	public class ImageDistributionService : IImageDistributionService
	{
		private readonly IUserService _userService;

		private readonly IMassTransitSignalRBackplaneService _massTransitSignalRBackplaneService;

		private readonly INotificationService _notificationService;

		private readonly IImageDistributionRepository _imageDistributionRepository;

		public ImageDistributionService(
			IImageEventHub imageEventHub,
			IUserService userService,
			IMassTransitSignalRBackplaneService massTransitSignalRBackplaneService,
			INotificationService notificationService,
			IImageDistributionRepository imageDistributionRepository)
		{
			_userService = userService;
			_massTransitSignalRBackplaneService = massTransitSignalRBackplaneService;
			_notificationService = notificationService;
			_imageDistributionRepository = imageDistributionRepository;

			imageEventHub.ImageUploaded.Register(OnImageUploaded);
		}

		public async Task AcknowledgeReceiveForClient(PicroUser user, Guid imageId)
		{
			await _imageDistributionRepository.AcknowledgeReceival(imageId, user);
		}

		public async Task<IEnumerable<ImageShareInfo>> GetImagesSharedToUser(PicroUser user)
		{
			var images = await _imageDistributionRepository.GetImageMappings(user);

			return images
				.Select(mapping => new ImageShareInfo(mapping.Image!.ImageId, mapping.Image.ImageLink));
		}

		private async Task OnImageUploaded(ImageUploadedEvent imageUploadedEvent)
		{
			var (uploader, imageUri, imageId) = imageUploadedEvent;

			var recipients = (await _userService.GetRandomUsers(uploader.Identifier)).ToList();

			var imageInfo = new ImageShareInfo(imageId, imageUri);
			var notification = FrontendNotificationFactory.Create(imageInfo, NotificationType.ImageShared);

			await DistributeToClientOverSignalR(imageId, recipients, notification);

			BackgroundJob.Schedule(() => ProcessShareFeedback(imageId, notification), TimeSpan.FromSeconds(90));
		}

		private async Task DistributeToClientOverSignalR<T>(Guid imageId, List<PicroUser> receivers, FrontendNotification<T> notification)
		{
			// Notify all clients
			await receivers.ParallelAsync(receiver =>
				_massTransitSignalRBackplaneService.RaiseGroupSignalREvent(receiver.Identifier.ToString(), notification));

			// Add all clients
			await _imageDistributionRepository.InsertMappings(imageId, receivers);
		}

		/// <summary>
		/// Processes the feedback for the image share, and contacts all unreachable clients via WebPush
		/// </summary>
		public async Task ProcessShareFeedback<T>(Guid imageId, FrontendNotification<T> notification)
		{
			var receivers = (await _imageDistributionRepository.GetImageMappings(imageId))
				.Where(x => !x.Acknowledged && x.User!.ConnectionInformation?.NotificationSubscription != null)
				.Select(x => x.User)
				.ToList();

			if (receivers.IsNullOrEmpty())
			{
				return;
			}

			await receivers.ParallelAsync(receiver => _notificationService.SendNotificationToSession(receiver, notification));
		}
	}
}