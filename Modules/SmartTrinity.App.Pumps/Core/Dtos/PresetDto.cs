using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Pumps.Core.Dtos
{
    public record PresetDto
    {
        public int Type { get; set; }
        public int PumpNo { get; set; }
        public double Amount { get; set; }
        public bool TankFull { get; set; };
        public IEnumerable<int>? Grades { get; set; }
    }
}
