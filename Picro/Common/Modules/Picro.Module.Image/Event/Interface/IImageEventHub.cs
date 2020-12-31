using Picro.Common.Eventing.Events.Interface;
using Picro.Module.Image.DataTypes;

namespace Picro.Module.Image.Event.Interface
{
    public interface IImageEventHub
    {
        IDistributedEvent<ImageUploadedEvent> ImageUploaded { get; }
    }
}