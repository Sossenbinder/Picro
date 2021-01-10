using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Picro.Module.Image.Service.Interface;
using Picro.Module.User.Service.Interface;

namespace Picro.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AckController : SessionBaseController
    {
        private readonly IImageDistributionService _imageDistributionService;

        public AckController(
            IUserService userService,
            IImageDistributionService imageDistributionService)
            : base(userService)
        {
            _imageDistributionService = imageDistributionService;
        }

        [HttpPost]
        [Route("AckImageShare")]
        public async Task AckImageShare([FromQuery] Guid imageId)
        {
            await _imageDistributionService.AcknowledgeReceiveForClient(User, imageId);
        }
    }
}