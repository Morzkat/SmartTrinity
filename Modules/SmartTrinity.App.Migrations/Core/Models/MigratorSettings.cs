using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Migrations.Core.Models
{
    public class MigratorSettings
    {
        public string SalesConnectionString { get; set; }
        public string PaymentsConnectionString { get; set; }
        public string DepositsConnectionString { get; set; }
    }
}
