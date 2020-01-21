using System;
using System.Collections.Generic;
using SmartTrinityConsole.Core.Entities.Pump;
using SmartTrinityConsole.Core.Entities.Client;
using SmartTrinityConsole.Infrastructure.Persistence;
using SmartTrinityConsole.Core.Entities.Sale;

namespace SmartTrinityConsole.Infrastructure.Client.Notifications.SignalR
{
      public class NotifyChangesToClient : INotifyClient<Pump>
      {
            public string EndPoint { get; private set; } = "StatusChange";

            private Dictionary<string, string> _data;

            //TODO: Refactor namespace
            public Pump GetClientData()
            {
                  return new Pump
                  {
                        PumpNo = Convert.ToInt32(_data["pump"]),
                        Status = _data["ST"],
                        SaleProgress = 0
                  };
            }

            public NotifyChangesToClient()
            {
            }

            public NotifyChangesToClient(Dictionary<string, string> data)
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
                  return new Pump
                  {
                        Status = "FUELLING",
                        Volume = Convert.ToDouble(_data["VO"]),
                        PumpNo = Convert.ToInt32(_data["pump"]),
                        SalePrice = Convert.ToDouble(_data["PU"]),
                        SaleProgress = Convert.ToDouble(_data["AM"]),
                        Grade = SmartPumpPersistence.GetPump(Convert.ToInt32(_data["pump"])).Grade
                  };
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
