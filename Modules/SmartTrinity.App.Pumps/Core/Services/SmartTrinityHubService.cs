using SmartTrinity.App.Pumps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Pumps.Core.Services
{
    public interface ISmartTrinityHubService
    {
        public Task PumpStatusChangeNotification(Pump pump);
        public Task PumpDeliveryProgressNotification(Pump pump);
    }
}
