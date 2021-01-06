using Picro.Module.User.DataTypes.Entity;
using System;

namespace Picro.Module.User.DataTypes
{
	public class User
	{
		public Guid Identifier { get; set; }

		public DateTime LastAccessedAtUtc { get; set; }

		public UserEntity ToEntity()
		{
			return new()
			{
				UserId = Identifier,
				LastAccessedAt = LastAccessedAtUtc,
			};
		}
	}
}