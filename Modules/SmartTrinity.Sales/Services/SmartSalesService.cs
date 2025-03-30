using SmartTrinity.Shared.Core.Models;
using SmartTrinity.Shared.Core.Database;
using SmartTrinity.App.Sales.Core.Models.Filters;

namespace SmartTrinity.App.Sales.Services
{
    public interface ISmartSalesService
    {
        Task<IEnumerable<Sale>> GetSales(SaleQueryFilter queryFilter);
    }

    public class SmartSalesService : ISmartSalesService
    {
        public ISalesUnitOfWork _salesUnitOfWork { get; set; }

        public ISmartSalesUnitOfWork _smartSalesUnitOfWork { get; set; }

        public SmartSalesService(ISmartSalesUnitOfWork salesUnitOfWork, ISmartSalesUnitOfWork smartSalesUnitOfWork)
        {
            _salesUnitOfWork = salesUnitOfWork;
            _smartSalesUnitOfWork = smartSalesUnitOfWork;
        }

        public async Task<IEnumerable<Sale>> GetSales(SaleQueryFilter queryFilter)
        {
            return await _smartSalesUnitOfWork.SmartSalesRepository.GetSales(queryFilter);
        }
    }
}
