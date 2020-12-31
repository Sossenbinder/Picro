using System;

namespace Picro.Module.Image.DataTypes
{
    public class UserImageMapping
    {
        public Guid UserIdentifier { get; set; }

        public Guid ImageIdentifier { get; set; }

        public string ImageUri { get; set; }
    }
}