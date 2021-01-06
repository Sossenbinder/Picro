using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Picro.Module.User.DataTypes.Entity;

namespace Picro.Module.Notification.DataTypes.Entity
{
	[Table("NotificationSubscription")]
	public class NotificationSubscriptionEntity
	{
		[Key]
		[Column("UserId")]
		[ForeignKey(nameof(User))]
		public Guid UserId { get; set; }

		public UserEntity User { get; set; }

		[Column("Url")]
		public string Url { get; set; }

		[Column("P256dh")]
		public string P256dh { get; set; }

		[Column("Auth")]
		public string Auth { get; set; }

		public NotificationSubscription ToModel()
		{
			return new()
			{
				Auth = Auth,
				Url = Url,
				P256dh = P256dh,
			};
		}
	}
}