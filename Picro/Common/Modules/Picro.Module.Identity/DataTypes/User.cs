using System;
using Picro.Common.Storage.Utils;
using Picro.Module.Identity.DataTypes.Entity;

namespace Picro.Module.Identity.DataTypes
{
    public class User
    {
        public Guid Identifier { get; set; }

        public DateTime LastAccessedAtUtc { get; set; }

        public string? ConnectionId { get; set; }

        public UserEntity ToEntity()
        {
            var identifierStringified = Identifier.ToString();

            return new UserEntity()
            {
                Identifier = Identifier,
                LastAccessedAtUtc = LastAccessedAtUtc,
                ETag = "*",
                RowKey = identifierStringified,
                PartitionKey = identifierStringified
            };
        }
    }
}