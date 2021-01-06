using System;
using System.ComponentModel.DataAnnotations.Schema;
using Picro.Module.User.DataTypes.Entity;

namespace Picro.Module.Image.DataTypes.Entity
{
	[Table("ImageUserMapping")]
	public class ImageUserMappingEntity
	{
		[Column("UserId")]
		[ForeignKey(nameof(User))]
		public Guid UserId { get; set; }

		public UserEntity User { get; set; }

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