using System;
using System.Collections.Generic;
using SmartTrinityConsole.Core.Entities.Pump;
using SmartTrinityConsole.Core.Entities.Sale;

namespace SmartTrinityConsole.Infrastructure.Persistence
{
    public static class SmartPumpPersistence
    {
        private static List<Pump> _pumps = new List<Pump>();
        public static DateTime LastUpdate { get; set; }

        public static List<Pump> GetPumps()
        {
            return _pumps;
        }

        public static Pump GetPump(int pumpId)
        {
            bool exist = _pumps.Exists(x => x.PumpNo == pumpId);
            if (exist)
                return _pumps.Find(x => x.PumpNo == pumpId);

            return null;
        }

        public static void AddPump(int pumpId)
        {
            _pumps.Add(new Pump
            {
                PumpNo = pumpId,
                Status = "IDLE",
                Volume = 0.0,
                SalePrice = 0.0,
                SaleProgress = 0.0,
                Hoses = new List<Hose>()
            });
        }

        public static void UpdatePump(Pump pump)
        {
            bool exist = _pumps.Exists(x => x.PumpNo == pump.PumpNo);
            if (exist)
            {
                int index = _pumps.FindIndex(p => p.PumpNo == pump.PumpNo);
                _pumps[index] = pump;
            }
        }

        public static void RemovePersistence()
        {
            _pumps = new List<Pump>();
        }
    }
}