using SmartTrinity.App.Pumps.Core.Models;

namespace SmartTrinity.App.Pumps.Core.Services
{
    public interface IPumpsService
    {
        Task SendPresent(Preset preset);
        Task ExecuteAction(PumpAction pumpAction);
        Task<bool> UpdateServiceMode(ServiceMode pumpServiceMode);
    }
}
