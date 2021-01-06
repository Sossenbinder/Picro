using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Picro.Module.User.DataTypes.Entity
{
	[Table("Users")]
	public class UserEntity
	{
		[Key]
		[Column("UserId")]
		public Guid UserId { get; set; }

		[Column("LastAccessedAtUtc")]
		public DateTime LastAccessedAt { get; set; }

		public User ToUserModel()
		{
			return new()
			{
				Identifier = UserId,
				LastAccessedAtUtc = LastAccessedAt,
			};
		}
	}
}