using SmartTrinityConsole.Core.Entities.Sale;
using SmartTrinityConsole.Core.Entities.ServerResponse;

namespace SmartTrinityApi.Core.Interfaces.Services
{
    public interface IPumpService
    {
        void PumpsBaseConfig();
        Result<ResponseWithList<Sale>> GetSales(int pumpNo);
        Result<Response> SendPresent(PresetConfig presetConfig);
        Result<Response> ExecutePumpAction(PumpAction pumpAction);
        Result<ResponseWithList<PumpServiceMode>> GetPumpsAndServicesModes();
        Result<Response> UpdatePumpServiceMode(PumpServiceMode pumpServiceMode);
    }
}
