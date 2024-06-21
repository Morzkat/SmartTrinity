using SmartTrinity.App.Pumps.Core.Dtos;
using SmartTrinity.App.Pumps.Core.Models;

namespace SmartTrinity.App.Pumps.Core.Services
{
    public interface IPumpsTestService
    {
        Task<string> SendTestPresent(int pumpNo);
    }
}
