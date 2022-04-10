using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrinityApi.Core.Entities.DTOs
{
    public class PumpServiceModeDto
    {
        public int Id { get; set; }
        public int Pump { get; set; }
        public string ServiceMode { get; set; }
    }
}
