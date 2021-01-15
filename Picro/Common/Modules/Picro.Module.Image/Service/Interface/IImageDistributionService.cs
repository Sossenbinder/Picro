using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Picro.Module.Image.DataTypes.Notification;
using Picro.Module.User.DataTypes;

namespace Picro.Module.Image.Service.Interface
{
	public interface IImageDistributionService
	{
		Task AcknowledgeReceiveForClient(PicroUser user, Guid imageId);

		Task<IEnumerable<ImageShareInfo>> GetImagesSharedToUser(PicroUser user);
	}
}