using SmartTrinity.Core.Database;
using SmartTrinity.App.Prices.Core.Models;

namespace SmartTrinity.App.Prices.Core.Database.Repositories
{
    public interface IPriceDetailsRepository: IRepository<PriceDetails>
    {
        Task<IList<PriceDetails>> GetPriceDetails(int priceId);
    }
}
