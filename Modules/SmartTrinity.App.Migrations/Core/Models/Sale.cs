using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Migrations.Core.Models
{
    public class Sale : Sales.Core.Models.Sale
    {
        public int StationId { get; set; }
    }
}
