using SmartTrinity.App.Prices.Core.Database;
using SmartTrinity.App.Prices.Core.Services;

namespace SmartTrinity.App.Prices.Services
{
    public class PriceService : IPriceService
    {

        public PriceService(IPriceUnitOfWork priceUnitOfWork)
        {
            
        }
        public Task SavePrice()
        {
            throw new NotImplementedException();
        }
    }
}
