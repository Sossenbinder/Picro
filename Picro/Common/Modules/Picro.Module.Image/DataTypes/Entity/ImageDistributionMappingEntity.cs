using Picro.Module.User.DataTypes.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Picro.Module.Image.DataTypes.Entity
{
	[Table("ImageDistributionMapping")]
	public class ImageDistributionMappingEntity
	{
		[Column("ImageId")]
		[Key]
		[ForeignKey(nameof(ImageInfo))]
		public Guid ImageId { get; set; }

		public UploadedImageInfoEntity? ImageInfo { get; set; }

		[Column("UserId")]
		[ForeignKey(nameof(User))]
		public Guid UserId { get; set; }

		public UserEntity? User { get; set; }

		[Column("TimestampUtc")]
		public DateTime Timestamp { get; set; }

		[Column("Acknowledged")]
		public bool Acknowledged { get; set; }

		public ImageDistributionMapping ToModel()
		{
			return new()
			{
				User = User?.ToUserModel(),
				Acknowledged = Acknowledged,
				Image = ImageInfo?.ToModel(),
				Timestamp = Timestamp,
			};
		}
	}
}