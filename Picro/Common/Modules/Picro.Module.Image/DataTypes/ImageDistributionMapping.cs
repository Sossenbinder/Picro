using System;
using Picro.Module.User.DataTypes;

namespace Picro.Module.Image.DataTypes
{
    public class ImageDistributionMapping
    {
        public Guid ImageId { get; set; }

        public PicroUser User { get; set; }

        public DateTime Timestamp { get; set; }

        public bool Acknowledged { get; set; }
    }
}