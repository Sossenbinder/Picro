using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Picro.Module.Image.DataTypes.Entity
{
    [Table("ImageUserMapping")]
    public class ImageUserMappingEntity
    {
        [Column("UserId")]
        public Guid UserId { get; set; }

        [Column("ImageId")]
        public Guid ImageId { get; set; }

        [Column("ImageLink")]
        public string ImageLink { get; set; }

        [Column("UploadTimestamp")]
        public DateTime UploadTimeStamp { get; set; }

        public ImageUserMapping ToModel()
        {
            return new()
            {
                UserId = UserId,
                ImageId = ImageId,
                ImageLink = ImageLink,
            };
        }
    }
}