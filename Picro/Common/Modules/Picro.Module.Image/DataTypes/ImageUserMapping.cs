using System;

namespace Picro.Module.Image.DataTypes
{
    public class ImageUserMapping
    {
        public Guid UserId { get; set; }

        public Guid ImageId { get; set; }

        public string ImageLink { get; set; }
    }
}