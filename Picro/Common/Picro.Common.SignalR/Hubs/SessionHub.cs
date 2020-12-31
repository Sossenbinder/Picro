using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Picro.Common.SignalR.Hubs.Interface;
using Picro.Module.Identity.Cache.Interface;

namespace Picro.Common.SignalR.Hubs
{
    public class SessionHub : Hub<ISessionHub>
    {
        private readonly IConnectedUserCache _connectedUserCache;

        private readonly string _identificationCookieName;

        public SessionHub(
            IConnectedUserCache connectedUserCache,
            IConfiguration configuration)
        {
            _connectedUserCache = connectedUserCache;
            _identificationCookieName = configuration["IdentificationCookieName"];
        }

        public override Task OnConnectedAsync()
        {
            _connectedUserCache.UpdateUserConnectionId(GetClientId(), Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _connectedUserCache.UpdateUserConnectionId(GetClientId(), null);
            return base.OnDisconnectedAsync(exception);
        }

        private Guid GetClientId()
        {
            var clientIdentifier = Context.GetHttpContext().Request.Cookies[_identificationCookieName];

            return Guid.Parse(clientIdentifier!);
        }
    }
}