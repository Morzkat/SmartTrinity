using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrinityApi.Core.Interfaces.Process
{
    public interface IPumpProcess
    {
        void AddPump(int pumpId);
        void CreatePump(int pumpId);
        void DestroyPump(int pumpId);
    }
}
