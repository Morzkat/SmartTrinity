using SmartTrinity.Core.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Prices.Core.Models
{
    public class PriceDetails: BaseEntity
    {
        public int GradeId { get; set; }
        public int PriceLevel { get; set; }
        public double Ppu { get; set; }
        public required string Product { get; set; }
    }
}
