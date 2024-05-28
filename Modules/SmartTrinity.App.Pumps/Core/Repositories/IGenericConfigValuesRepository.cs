using SmartTrinity.App.Pumps.Core.Models;
using SmartTrinity.Core.Database;

namespace SmartTrinity.App.Pumps.Core.Repositories
{
    public interface IGenericConfigValuesRepository : IRepository<GenericConfigValues>
    {
        Task<bool> UpdatePumpServiceMode(ServiceMode serviceMode);

        Task<GenericConfigValues> GetServiceModeByDeviceId(int id);
    }
}