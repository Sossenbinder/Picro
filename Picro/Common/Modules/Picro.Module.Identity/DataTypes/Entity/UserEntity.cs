using System;
using Microsoft.Azure.Cosmos.Table;

namespace Picro.Module.Identity.DataTypes.Entity
{
    public class UserEntity : TableEntity
    {
        public string UnderlyingIdentifier { get; set; }

        [IgnoreProperty]
        public Guid Identifier
        {
            get => Guid.Parse(UnderlyingIdentifier);
            set => UnderlyingIdentifier = value.ToString();
        }

        public User ToUserModel()
        {
            return new()
            {
                Identifier = Identifier,
            };
        }
    }
}