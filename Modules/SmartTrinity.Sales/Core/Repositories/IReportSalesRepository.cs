using SmartTrinity.App.Sales.Core.Models;
using SmartTrinity.Core.Database;
using SmartTrinity.Shared.Core.Models;

namespace SmartTrinity.App.Sales.Core.Repositories
{
    public interface IReportSalesRepository: IRepository<ReportSale>
    {
        Task<int> GetLastId();
        Task<List<Sale>> GetReportSalesFromId(int saleId);
    }
}
