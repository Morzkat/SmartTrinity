using System.Collections.Generic;
using SmartTrinityConsole.Core.Entities.Database.Configs;
using SmartTrinityConsole.Core.Entities.Pump;

namespace SmartTrinityApi.Core.Interfaces.Repository.Repositories
{
    public interface IConfigValuesRepository : IRepository<ConfigValues>
    { 
        IEnumerable<PumpServiceMode> GetPumpsAndServicesModes();
        void UpdatePumpServiceMode();
    }
}