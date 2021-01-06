using System;
using Picro.Module.User.DataTypes;

namespace Picro.Module.Image.DataTypes
{
	public record ImageUploadedEvent(User Uploader, string ImageUri, Guid ImageId);
}