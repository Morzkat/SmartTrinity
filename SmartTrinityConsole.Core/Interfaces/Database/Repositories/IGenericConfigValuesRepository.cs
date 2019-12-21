using SmartTrinityConsole.Core.Entities.Database.Configs;
using SmartTrinityConsole.Core.Entities.Pump;

namespace SmartTrinityApi.Core.Interfaces.Repository.Repositories
{
    public interface IGenericConfigValuesRepository : IRepository<GenericConfigValues>
    {
        bool UpdatePumpServiceMode(PumpServiceMode pumpServiceMode);
    }
}