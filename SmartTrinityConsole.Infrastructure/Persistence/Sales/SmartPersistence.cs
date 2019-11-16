using System.Collections.Generic;
using SmartTrinityConsole.Core.Entities.Pump;
using SmartTrinityConsole.Core.Entities.Sale;

namespace SmartTrinityConsole.Infrastructure.Persistence
{
    public static class SmartPersistence
    {
        private static IList<IList<Sale>> _sales = new List<IList<Sale>>();
        private static List<Pump> _pumps = new List<Pump>();

        public static List<Pump> GetPumps()
        {
            return _pumps;
        }

        public static void AddPump(int pumpId)
        {
            _pumps.Add(new Pump
            {
                PumpNo = pumpId,
                Status = "OFFLINE",
                Volume = 0.0,
                SalePrice = 0.0,
                SaleProgress = 0.0
            });
        }

        public static void AddNewListPumpSales()
        {
            _sales.Add(new List<Sale>());
        }

        public static void AddSales(int pumpId, IList<Sale> sales)
        {
            _sales[pumpId] = sales;
        }

        public static IList<IList<Sale>> GetSales()
        {
            return _sales;
        }
    }
}