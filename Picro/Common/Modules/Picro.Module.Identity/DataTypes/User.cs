using System;

namespace Picro.Module.Identity.DataTypes
{
    public class User
    {
        public Guid Identifier { get; set; }

        public string? ConnectionId { get; set; }
    }
}