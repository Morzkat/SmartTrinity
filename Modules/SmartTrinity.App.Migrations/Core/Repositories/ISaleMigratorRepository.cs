using SmartTrinity.Shared.Core.Database.Repositories;
using SmartTrinity.Shared.Core.Models;

namespace SmartTrinity.App.Migrations.Core.Repositories
{
    public interface ISalesMigratorRepository : ISalesRepository
    {
        Task<int> AddToCentralServer(int stationId, Sale sale);
    }
}
