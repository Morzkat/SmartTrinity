using System;
using System.Collections.Generic;
using System.Text;
using SmartTrinityConsole.Core.Entities.Pump;

namespace SmartTrinityApi.Core.Interfaces.Services
{
    public interface IMainService
    {
        string ExecutePumpAction(PumpAction pumpAction);
        void ReadFromSocket();
    }
}
