using System;
using System.Threading.Tasks;
using Picro.Module.User.DataTypes;

namespace Picro.Module.Image.Service.Interface
{
    public interface IImageDistributionService
    {
        Task AcknowledgeReceiveForClient(PicroUser user, Guid imageId);
    }
}