using SmartTrinityConsole.Core.Entities.Pump;

namespace SmartTrinityConsole.Core.Entities.Sale
{
    public class Sale
    {
        public int Pump { get; set; }
        public int Hose { get; set; }
        public int Type { get; set; }
        public int SaleId { get; set; }
        public double PPU { get; set; }
        public string RGB { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public double Amount { get; set; }
        public double Volume { get; set; }
        public string TypeDescription { get; set; }
    }
}