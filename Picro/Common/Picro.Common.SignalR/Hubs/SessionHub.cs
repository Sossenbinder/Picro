using Microsoft.AspNetCore.SignalR;
using Picro.Common.SignalR.Caches.Interface;
using Picro.Common.SignalR.Hubs.Interface;
using Picro.Module.User.Service.Interface;
using System;
using System.Threading.Tasks;

namespace Picro.Common.SignalR.Hubs
{
	public class SessionHub : Hub<ISessionHub>
	{
		private readonly IUserService _userService;

		private readonly IConnectedGroupCache _connectedGroupCache;

		public SessionHub(
			IUserService userService,
			IConnectedGroupCache connectedGroupCache)
		{
			_userService = userService;
			_connectedGroupCache = connectedGroupCache;
		}

		public override async Task OnConnectedAsync()
		{
			var userId = GetUserId();
			await Groups.AddToGroupAsync(Context.ConnectionId, GetUserId().ToString());
			_connectedGroupCache.Connect(userId);
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			var userId = GetUserId();

			_connectedGroupCache.Disconnect(userId);

			return _userService.UpdateUser(userId, x =>
			{
				x.LastAccessedAtUtc = DateTime.UtcNow;
			});
		}

		private Guid GetUserId() => Guid.Parse(Context.User.Identity!.Name!);
	}
}