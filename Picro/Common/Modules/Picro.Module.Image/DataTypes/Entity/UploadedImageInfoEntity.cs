using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Picro.Module.User.DataTypes.Entity;

namespace Picro.Module.Image.DataTypes.Entity
{
    [Table("UploadedImageInfos")]
    public class UploadedImageInfoEntity
    {
        [Column("UserId")]
        [Key]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public UserEntity User { get; set; }

        [Column("ImageId")]
        public Guid ImageId { get; set; }

        [Column("ImageLink")]
        public string ImageLink { get; set; }

        [Column("UploadTimestamp")]
        public DateTime UploadTimeStamp { get; set; }

        public UploadedImageInfo ToModel()
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