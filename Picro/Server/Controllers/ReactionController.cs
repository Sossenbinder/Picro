using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Picro.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReactionController : ControllerBase
    {
        private readonly ILogger<ReactionController> _logger;

        public ReactionController(ILogger<ReactionController> logger)
        {
            _logger = logger;
        }
    }
}