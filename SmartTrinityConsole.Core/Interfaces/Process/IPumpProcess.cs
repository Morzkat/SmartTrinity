using System;
using System.Collections.Generic;
using System.Text;
using SmartTrinityApi.Core.Entities.Pump;

namespace SmartTrinityApi.Core.Interfaces.Process
{
    public interface IPumpProcess
    {
        void AddPump(int pumpId);
        void CreatePump(int pumpId);
        void DestroyPump(int pumpId);
       void ExecutePumpAction(PumpAction pumpAction);
    }
}
