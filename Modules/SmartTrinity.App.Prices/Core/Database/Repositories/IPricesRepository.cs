using SmartTrinity.Core.Database;
using SmartTrinity.App.Prices.Core.Models;

namespace SmartTrinity.App.Prices.Core.Database.Repositories
{
    public interface IPricesRepository: IRepository<Price>
    {
        Task<IList<Price>> GetPrices(int offset, int limit);
        Task<int> GetNextPriceChangeId();
    }
}
