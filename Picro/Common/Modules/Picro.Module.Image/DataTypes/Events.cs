using Picro.Module.Identity.DataTypes;

namespace Picro.Module.Image.DataTypes
{
    public record ImageUploadedEvent(User Uploader, string ImageUri);
}