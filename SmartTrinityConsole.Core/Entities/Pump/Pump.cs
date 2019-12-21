using System.Collections.Generic;

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

    public class PumpServiceMode
    {
        public int Id { get; set; }
        public int Pump { get; set; }
        public string ServiceMode { get; set; }
    }

    /**
        type 1 = MONEY
        type 2 = VOLUME
    **/
    public class PresetConfig
    {
        public int Type { get; set; }
        public int PumpNo { get; set; }
        public double Amount { get; set; }
        public bool TankFull { get; set; }
        public IList<Grade> Grades { get; set; }
    }

    public class PumpAction
    {
        public int Pump { get; set; }
        public string Action { get; set; }
    }
}