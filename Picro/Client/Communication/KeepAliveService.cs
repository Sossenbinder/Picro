using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Picro.Client.Communication.Interface;

namespace Picro.Client.Communication
{
    public class KeepAliveService : IKeepAliveService
    {
        private readonly HubConnection _hubConnection;

        public KeepAliveService(IConfiguration configuration)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{configuration["RemoteEndpoint"]}/SessionHub")
                .Build();
        }

        public async Task InitializeConnection()
        {
            try
            {
                await _hubConnection.StartAsync();
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("Failed to initialize websocket connection, will revert to long polling...");
            }
        }
    }
}