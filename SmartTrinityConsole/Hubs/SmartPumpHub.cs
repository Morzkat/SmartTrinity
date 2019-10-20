using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SmartTrinityApi.Hubs
{
    //TODO: 
    public class SmartPumpHub : Hub
    {
        public async Task SendMessage(string user)
        {
            await Clients.All.SendAsync("PumpDeliveryProgress", user);
        }
    }
}