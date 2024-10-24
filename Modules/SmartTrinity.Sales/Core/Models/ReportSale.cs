using SmartTrinity.Core.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Sales.Core.Models
{
    public class ReportSale : BaseEntity
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
        public int Shift { get; set; }
        public string Day {  get; set; }
    }
}
