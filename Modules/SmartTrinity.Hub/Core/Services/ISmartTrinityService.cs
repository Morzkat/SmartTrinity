using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Core.Services
{
    public interface ISmartTrinityService
    {
        void Setup();
        void SetupRequestConfigurations();
        void SetupSubscriptionsToEvents();
    }
}
