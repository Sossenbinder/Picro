using System;
using Picro.Module.User.DataTypes;

namespace Picro.Module.Image.DataTypes
{
	public record ImageUploadedEvent(PicroUser Uploader, string ImageUri, Guid ImageId);
}