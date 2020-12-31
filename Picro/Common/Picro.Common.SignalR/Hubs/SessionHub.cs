using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Picro.Common.SignalR.Hubs.Interface;
using Picro.Module.Identity.Cache.Interface;
using Picro.Module.Identity.Service.Interface;

namespace Picro.Common.SignalR.Hubs
{
    public class SessionHub : Hub<ISessionHub>
    {
        private readonly IUserService _userService;

        private readonly string _identificationCookieName;

        public SessionHub(
            IUserService userService,
            IConfiguration configuration)
        {
            _userService = userService;
            _identificationCookieName = configuration["IdentificationCookieName"];
        }

        public override Task OnConnectedAsync()
        {
            return _userService.UpdateUser(GetClientId(), x => x.ConnectionId = Context.ConnectionId);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var clientId = GetClientId();

            return _userService.UpdateUser(clientId, x =>
            {
                x.ConnectionId = null;
                x.LastAccessedAtUtc = DateTime.UtcNow;
            });
        }

        private Guid GetClientId()
        {
            var clientIdentifier = Context.GetHttpContext().Request.Cookies[_identificationCookieName];

            return Guid.Parse(clientIdentifier!);
        }
    }
}