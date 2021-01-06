using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Picro.Module.User.Service.Interface;
using Picro.Module.Notification.DataTypes;
using Picro.Module.Notification.Service.Interface;
using Picro.Server.Utils;

namespace Picro.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class NotificationsController : SessionBaseController
	{
		private readonly INotificationService _notificationService;

		public NotificationsController(
			INotificationService notificationService,
			IUserService userService)
			: base(userService)
		{
			_notificationService = notificationService;
		}

		[HttpPost("Register")]
		[Authorize]
		public async Task<JsonResponse> Register([FromBody] NotificationSubscription notificationSubscription)
		{
			var success = await _notificationService.RegisterUserForNotifications(User, notificationSubscription);

			return success ? JsonResponse.Success() : JsonResponse.Error();
		}
	}
}