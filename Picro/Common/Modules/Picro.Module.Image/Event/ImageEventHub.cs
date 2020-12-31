using Microsoft.Extensions.Logging;
using Picro.Common.Eventing.Events;
using Picro.Common.Eventing.Events.Interface;
using Picro.Common.Eventing.Events.MassTransit.Interface;
using Picro.Module.Image.DataTypes;
using Picro.Module.Image.Event.Interface;

namespace Picro.Module.Image.Event
{
    public class ImageEventHub : IImageEventHub
    {
        public IDistributedEvent<ImageUploadedEvent> ImageUploaded { get; }

        public ImageEventHub(
            IMassTransitEventingService massTransitEventingService,
            ILogger<ImageEventHub> logger)
        {
            ImageUploaded = new MassTransitEvent<ImageUploadedEvent>(nameof(ImageUploaded), massTransitEventingService, logger);
        }
    }
}