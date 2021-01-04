using Picro.Common.Eventing.Events;

namespace Picro.Client.Services.Interface
{
    public interface IDistributedImageService
    {
        AsyncEvent ImageReceived { get; }
    }
}