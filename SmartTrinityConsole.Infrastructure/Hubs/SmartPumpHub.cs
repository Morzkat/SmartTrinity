using Microsoft.AspNetCore.SignalR;
using SmartTrinityApi.Core.Interfaces.Hubs;
using SmartTrinityConsole.Core.Entities.Pump;
using SmartTrinityConsole.Core.Entities.Sale;
using SmartTrinityConsole.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartTrinityApi.Infrastructure.Hubs
{

    public interface ISmartPump
    {
        Task LoadPumps(List<Pump> pumps);
        Task LoadLatestPumpSales(IList<IList<Sale>> sales);
        /*Task StatusChange();
        Task LatestPumpSales();
        Task UpdatePumpSales();
        Task PumpDeliveryProgress();*/
    }

    public class SmartPumpHub : Hub<ISmartPump>
    {
        public Task LoadPumps()
        {
            return Clients.Caller.LoadPumps(SmartPumpPersistence.GetPumps());
        }

        public Task LoadLatestPumpSales()
        {
            return Clients.Caller.LoadLatestPumpSales(SmartSalePersistence.GetSales());
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