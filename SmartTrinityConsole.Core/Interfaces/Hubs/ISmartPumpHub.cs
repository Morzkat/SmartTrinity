using System.Threading.Tasks;

namespace SmartTrinityApi.Core.Interfaces.Hubs
{
    public interface ISmartPumpHub
    {
        Task SendAsync(string t, string t2);
    }
}