using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Pumps.Core.Models
{
    public enum ServiceModeTypes
    {
        SELF_SERVICE,
        FULL_SERVICE,
    }

    public class ServiceMode
    {
        public int Id { get; set; }
        public int Pump { get; set; }
        public ServiceModeTypes ServiceModeType { get; set; }
    }
}
