using SmartTrinity.App.Pumps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Pumps.Persistence
{
    public static class PumpsPersistence
    {
        private static List<Pump> _pumps = new List<Pump>();
        public static DateTime LastUpdate { get; set; }
        public static Dictionary<int, PumpActions> PumpWithAction = new Dictionary<int, PumpActions>();

        public static List<Pump> Get()
        {
            return _pumps;
        }

        public static Pump Get(int pumpId)
        {
            bool exist = _pumps.Exists(x => x.PumpNo == pumpId);
            if (exist)
                return _pumps.Find(x => x.PumpNo == pumpId);

            return null;
        }

        public static void Add(int pumpId)
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

        public static void Update(Pump pump)
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

        public static PumpActions GetAction(int pump)
        {
            try { return PumpWithAction[pump]; }
            catch { return PumpActions.DEAUTH; }
        }

        public static void AddAction(int pump, PumpActions action)
        {
            PumpWithAction[pump] = action;
        }

        public static void RemoveAction(int pump)
        {
            PumpWithAction.Remove(pump);
        }
    }
}
