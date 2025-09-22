using SmartTrinity.App.Prices.Core.Database.Repositories;

namespace SmartTrinity.App.Prices.Core.Database
{
    public interface IPriceUnitOfWork
    {
        IPricesRepository PricesRepository { get; }
        IPriceDetailsRepository PricesDetailsRepository { get; }
    }
}
