using System;
using System.Collections.Generic;
using System.Text;
using SmartTrinityConsole.Core.Entities.Pump;

namespace SmartTrinityApi.Core.Interfaces.Services
{
    public interface IPumpService
    {
        void PumpsBaseConfig();
        void SendPresent(PresetConfig presetConfig);
        void ExecutePumpAction(PumpAction pumpAction);
        List<PumpServiceMode> GetPumpsAndServicesModes();
        void UpdatePumpServiceMode(PumpServiceMode pumpServiceMode);
    }
}
