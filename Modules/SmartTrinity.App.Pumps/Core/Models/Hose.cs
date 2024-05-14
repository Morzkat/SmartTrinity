using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Pumps.Core.Models
{
    public class Hose
    {
        public int HoseId { get; set; }
        public double TotalizerMoney { get; set; }
        public double TotalizerVolume { get; set; }
        public IList<Grade> Grades { get; set; }
    }
}
