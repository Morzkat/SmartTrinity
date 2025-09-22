using SmartTrinity.Shared.Core.Models;
using SmartTrinity.Shared.Core.Database.Repositories;

namespace SmartTrinity.App.Sales.Core.Repositories
{
    public interface IAppSalesRepository : ISalesRepository
    {
        Task<int> AddToCentralServer(int stationId, Sale sale);
    }
}
