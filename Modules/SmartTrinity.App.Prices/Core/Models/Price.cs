using SmartTrinity.Core.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Prices.Core.Models
{
    public class Price: BaseEntity
    {
        public required string ApplicationDate { get; set; }
        public required string ApplicationTime { get; set; }
        public required string ProcessedDate { get; set; }
        public required string ProcessedTime { get; set; }
        public required List<PriceDetails> PriceDetails { get; set; }
    }
}
