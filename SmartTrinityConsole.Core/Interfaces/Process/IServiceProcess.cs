using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrinityApi.Core.Interfaces.Process
{
    public interface IServiceProcess
    {
        void AddPumpSalesProccess();
        void ProccessStationData();
        object ProcessMessage(string msgType, string msgData);
    }
}
