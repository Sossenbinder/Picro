using System;
using Microsoft.Azure.Cosmos.Table;

namespace Picro.Module.Image.DataTypes.Entity
{
    public class UserImageMappingEntity : TableEntity
    {
        public string UnderlyingUserIdentifier { get; set; }

        [IgnoreProperty]
        public Guid UserIdentifier
        {
            get => Guid.Parse(UnderlyingUserIdentifier);
            set => UnderlyingUserIdentifier = value.ToString();
        }

        public Guid ImageIdentifier { get; set; }

        public string ImageUri { get; set; }

        public UserImageMapping ToModel()
        {
            return new UserImageMapping()
            {
                ImageIdentifier = ImageIdentifier,
                ImageUri = ImageUri,
                UserIdentifier = UserIdentifier,
            };
        }
    }
}