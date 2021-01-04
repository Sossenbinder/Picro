using Picro.Module.Image.DataTypes.Enums;

namespace Picro.Module.Image.DataTypes.Response
{
    public record ImageUploadInfoResponse(ImageUploadErrorCode UploadErrorCode, ImageInfo? ImageInfo);
}