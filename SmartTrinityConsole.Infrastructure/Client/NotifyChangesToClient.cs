using System;
using System.Collections.Generic;
using SmartTrinityConsole.Core.Entities.Pump;
using SmartTrinityConsole.Core.Entities.Client;
using SmartTrinityConsole.Infrastructure.Persistence;
using SmartTrinityConsole.Core.Entities.Sale;

namespace SmartTrinityConsole.Infrastructure.Client.Notifications.SignalR
{
    public class NotifyPumpChangesToClient : INotifyClient<Pump>
    {
        public string EndPoint { get; private set; } = "PumpStatusChange";

        private Dictionary<string, string> _data;

        //TODO: Refactor namespace
        public Pump GetClientData()
        {
            int.TryParse(_data["pump"], out int pumpNo);

            Pump pump = SmartPumpPersistence.GetPump(pumpNo);
            pump.Status = _data["ST"];
            pump.SaleProgress = 0;

            return pump;
        }

        public NotifyPumpChangesToClient()
        {
        }

        public NotifyPumpChangesToClient(Dictionary<string, string> data)
        {
            _data = data;
        }
    }

    public class NotifyPumpDeliveryProgressToClient : INotifyClient<Pump>
    {
        public string EndPoint { get; private set; } = "PumpDeliveryProgress";

        private Dictionary<string, string> _data;

        //TODO: Refactor namespace
        public Pump GetClientData()
        {
            double.TryParse(_data["VO"], out double volume);
            int.TryParse(_data["pump"], out int pumpNo);
            double.TryParse(_data["PU"], out double salePrice);
            double.TryParse(_data["AMS"], out double saleProgress);

            Pump pump = SmartPumpPersistence.GetPump(pumpNo);

            pump.Status = "FUELLING";
            pump.Volume = volume;
            pump.PumpNo = pumpNo;
            pump.SalePrice = salePrice;
            pump.SaleProgress = saleProgress;
            pump.Grade = SmartPumpPersistence.GetPump(Convert.ToInt32(_data["pump"])).Grade;

            return pump;
        }

        public NotifyPumpDeliveryProgressToClient()
        {
        }

        public NotifyPumpDeliveryProgressToClient(Dictionary<string, string> data)
        {
            _data = data;
        }
    }

    public class NotifyLatestPumpSalesToClient : INotifyClient<IEnumerable<IEnumerable<Sale>>>
    {
        public string EndPoint { get; private set; } = "LoadLatestPumpSales";

        private Dictionary<string, string> _data;

        public IEnumerable<IEnumerable<Sale>> GetClientData() => SmartSalePersistence.GetSales();

        public NotifyLatestPumpSalesToClient()
        {
        }

        public NotifyLatestPumpSalesToClient(Dictionary<string, string> data)
        {
            _data = data;
        }
    }

    public class NotifyPumpsToClient : INotifyClient<IEnumerable<Pump>>
    {
        public string EndPoint { get; private set; } = "LoadPumps";

        private Dictionary<string, string> _data;

        public IEnumerable<Pump> GetClientData() => SmartPumpPersistence.GetPumps();

        public NotifyPumpsToClient()
        {
        }

        public NotifyPumpsToClient(Dictionary<string, string> data)
        {
            _data = data;
        }
    }

    // TODO: Look for a better name for the method...      
    public class NoNotificationToClient : INotifyClient<string>
    {
        public string EndPoint { get; private set; } = "";

        public string GetClientData() => "";
    }
}
