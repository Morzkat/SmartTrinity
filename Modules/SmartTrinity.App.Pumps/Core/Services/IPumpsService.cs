using SmartTrinity.App.Pumps.Core.Models;

namespace SmartTrinity.App.Pumps.Core.Services
{
    public interface IPumpsService
    {
        Task<string> SendPresent(Preset preset);
        Task<string> ExecuteAction(PumpAction pumpAction);
        Task<string> UpdateServiceMode(ServiceMode pumpServiceMode);
    }
}
