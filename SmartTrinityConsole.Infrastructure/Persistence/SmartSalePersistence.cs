using System.Collections.Generic;
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
    }
}