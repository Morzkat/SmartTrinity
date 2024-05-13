using  SmartTrinity.App.Sales.Core.Models;
using  SmartTrinity.App.Sales.Infrastructure.Repositories;

namespace  SmartTrinity.App.Sales.Services
{
    public interface ISalesService
    {
        Task<IEnumerable<Sale>> GetSales();
    }

    public class SalesService : ISalesService
    {
        private readonly ISalesUnitOfWork _salesUnitOfWork;

        public SalesService(ISalesUnitOfWork salesUnitOfWork)
        {
            _salesUnitOfWork = salesUnitOfWork;
        }

        public async Task<IEnumerable<Sale>> GetSales()
        {
            return await _salesUnitOfWork.SalesRepository.GetAll();
        }
    }
}