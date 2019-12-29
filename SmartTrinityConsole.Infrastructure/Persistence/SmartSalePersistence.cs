using System.Collections.Generic;
using System.Linq;
using SmartTrinityConsole.Core.Entities.Sale;

namespace SmartTrinityConsole.Infrastructure.Persistence
{
    public static class SmartSalePersistence
    {
        private static IList<IList<Sale>> _sales = new List<IList<Sale>>();

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

           public static IList<Sale> GetSalesByPump(int pumpNo)
        {
            return _sales[pumpNo];
        }

        public static void RemovePersistence()
        {
            _sales = new List<IList<Sale>>();
        }
    }
}