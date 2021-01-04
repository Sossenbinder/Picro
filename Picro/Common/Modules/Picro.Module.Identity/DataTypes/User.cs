using Picro.Module.Identity.DataTypes.Entity;
using System;

namespace Picro.Module.Identity.DataTypes
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