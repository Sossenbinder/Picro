using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Picro.Module.User.Service.Interface;

namespace Picro.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ReactionController : SessionBaseController
	{
		private readonly ILogger _logger;

		public ReactionController(
			IUserService userService,
			ILogger<ReactionController> logger)
			: base(userService)
		{
			_logger = logger;
		}
	}
}