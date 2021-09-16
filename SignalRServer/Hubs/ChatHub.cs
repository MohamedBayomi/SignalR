using System.Reflection;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRServer.Models;
using System.Text.Json;

namespace SignalRServer.Hubs
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("--> Connection Established " + Context.ConnectionId);
            Clients.Client(Context.ConnectionId).SendAsync("ReceiveConnID", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public async Task SendMessageAsync(string message)
        {
            System.Console.WriteLine($"--> Message Received: {message}");
            Payload payload = JsonSerializer.Deserialize<Payload>(message);
            if (string.IsNullOrWhiteSpace(payload.To) || payload.To == "*")
            {
                await Clients.All.SendAsync("ReceiveMessage", message);
            }
            else
            {
                var To = Clients.Client(payload.To);
                await To.SendAsync("ReceiveMessage", message);
            }
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}