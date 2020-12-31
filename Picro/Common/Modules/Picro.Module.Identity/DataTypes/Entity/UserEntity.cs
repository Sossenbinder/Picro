using System;
using Microsoft.Azure.Cosmos.Table;

namespace Picro.Module.Identity.DataTypes.Entity
{
    public class UserEntity : TableEntity
    {
        public Guid Identifier { get; set; }

        public DateTime LastAccessedAtUtc { get; set; }

        public User ToUserModel()
        {
            return new()
            {
                Identifier = Identifier,
                LastAccessedAtUtc = LastAccessedAtUtc,
            };
        }
    }
}