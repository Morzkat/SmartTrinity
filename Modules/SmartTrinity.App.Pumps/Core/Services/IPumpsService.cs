using SmartTrinity.App.Pumps.Core.Dtos;
using SmartTrinity.App.Pumps.Core.Models;

namespace SmartTrinity.App.Pumps.Core.Services
{
    public interface IPumpsService
    {
        IEnumerable<PumpDto> GetPumps();
        Task<string> SendPresent(PresetDto preset);
        Task<string> ExecuteAction(PumpAction pumpAction);
        Task<string> UpdateServiceMode(ServiceMode pumpServiceMode);
        void SetupPump(int pumpId);
    }
}
