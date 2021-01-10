using System;

namespace Picro.Module.Image.DataTypes
{
    public class UploadedImageInfo
    {
        public Guid UserId { get; set; }

        public Guid ImageId { get; set; }

        public string ImageLink { get; set; }
    }
}