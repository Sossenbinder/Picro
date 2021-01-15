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

		[InverseProperty("User")]
		public NotificationSubscriptionEntity? NotificationSubscription { get; set; }

		public PicroUser ToUserModel()
		{
			var user = new PicroUser()
			{
				Identifier = UserId,
				LastAccessedAtUtc = LastAccessedAt,
			};

			if (NotificationSubscription != null)
			{
				user.ConnectionInformation = new ConnectionInformation()
				{
					NotificationSubscription = NotificationSubscription.ToModel()
				};
			}

			return user;
		}
	}
}