using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Picro.Module.Image.DataTypes;
using Picro.Module.User.DataTypes;

namespace Picro.Module.Image.Storage.Interface
{
    public interface IImageDistributionRepository
    {
        Task InsertMapping(Guid imageId, PicroUser user);

        Task InsertMappings(Guid imageId, IEnumerable<PicroUser> users);

        Task AcknowledgeReceival(Guid imageId, PicroUser user);

        Task<IEnumerable<ImageDistributionMapping>> GetImageMappings(Guid imageId);
    }
}