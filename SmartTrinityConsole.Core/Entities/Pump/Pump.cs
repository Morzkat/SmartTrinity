namespace SmartTrinityConsole.Core.Entities.Pump
{
    public class Pump
    {
        public int PumpNo { get; set; } 
        public Grade Grade { get; set; }
        public string Status { get; set; }
        public double Volume { get; set; }
        public int PriceLevel { get; set; }
        public double SalePrice { get; set; }
        public double SaleProgress { get; set; }
    }
}