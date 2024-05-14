using SmartTrinity.App.Pumps.Core.Models;
using SmartTrinity.Core.Database;

namespace SmartTrinity.App.Pumps.Core.Repositories
{
    public interface IConfigValuesRepository : IRepository<ConfigValues>
    {
        Task<IEnumerable<ServiceMode>> GetPumpsAndServicesModes();
    }
}