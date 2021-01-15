using Microsoft.AspNetCore.SignalR;
using Picro.Common.SignalR.Hubs.Interface;
using Picro.Module.User.Service.Interface;
using System;
using System.Threading.Tasks;

namespace Picro.Common.SignalR.Hubs
{
	public class SessionHub : Hub<ISessionHub>
	{
		private readonly IUserService _userService;

		public SessionHub(IUserService userService)
		{
			_userService = userService;
		}

		public override Task OnConnectedAsync()
		{
			return Groups.AddToGroupAsync(Context.ConnectionId, GetUserId().ToString());
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			var userId = GetUserId();
			return _userService.UpdateUser(userId, x =>
			{
				x.LastAccessedAtUtc = DateTime.UtcNow;
			});
		}

		private Guid GetUserId() => Guid.Parse(Context.User.Identity!.Name!);
	}
}