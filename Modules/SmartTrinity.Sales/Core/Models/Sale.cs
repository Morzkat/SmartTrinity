

using SmartTrinity.Core.Database.Models;

namespace  SmartTrinity.App.Sales.Core.Models
{
    public class Sale : BaseEntity
    {
        public int PumpId { get; set; }
        public int HoseId { get; set; }
        public int GradeId { get; set; }
        public double Volume { get; set; }
        public double Money { get; set; }
        public double Ppu { get; set; }
        public double PresetAmount { get; set; }
        public string Product { get; set; }
        public int Level { get; set; }
        public int Saletype { get; set; }
        public double InitialVolume { get; set; }
        public double FinalVolume { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
    }
}

