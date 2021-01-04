using System;

namespace Picro.Module.Image.DataTypes.Response
{
    public record ImageInfo(Guid ImageId, string ImageLink, DateTime UploadTimeStamp);
}