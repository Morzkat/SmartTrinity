using SmartTrinity.App.Sales.Core.Models;
using SmartTrinity.Core.Database;

namespace SmartTrinity.App.Sales.Core.Repositories
{
    public interface IReportSalesRepository: IRepository<ReportSale>
    {
        Task<int> GetLastId();
        Task<object> GetReportSalesFromId(int saleId);
    }
}
