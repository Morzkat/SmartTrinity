using System;
using System.Collections.Generic;
using System.Text;
using SmartTrinityConsole.Core.Entities.Pump;
using SmartTrinityConsole.Core.Entities.Sale;
using SmartTrinityConsole.Core.ServerResponse;

namespace SmartTrinityApi.Core.Interfaces.Services
{
    public interface IPumpService
    {
        void PumpsBaseConfig();
        ResponseWithList<Sale> GetSales(int pumpNo);
        Response SendPresent(PresetConfig presetConfig);
        Response ExecutePumpAction(PumpAction pumpAction);
        ResponseWithList<PumpServiceMode> GetPumpsAndServicesModes();
        Response UpdatePumpServiceMode(PumpServiceMode pumpServiceMode);
    }
}
