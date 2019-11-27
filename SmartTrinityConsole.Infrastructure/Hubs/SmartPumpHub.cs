using Microsoft.AspNetCore.SignalR;
using SmartTrinityApi.Core.Interfaces.Hubs;
using SmartTrinityConsole.Core.Entities.Pump;
using SmartTrinityConsole.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartTrinityApi.Infrastructure.Hubs
{

    public interface ISmartPump
    {
        Task LoadPumps(List<Pump> pumps);
        /*Task StatusChange();
        Task LatestPumpSales();
        Task UpdatePumpSales();
        Task PumpDeliveryProgress();*/
    }

    public class SmartPumpHub : Hub<ISmartPump>
    {
        public Task LoadPumps()
        {
            return Clients.All.LoadPumps(SmartPumpPersistence.GetPumps());
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Clients");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}