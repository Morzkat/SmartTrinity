using SmartTrinity.App.Migrations.Core.Models;
using SmartTrinity.App.Migrations.Infrastructure;
using SmartTrinity.App.Sales.Infrastructure.Repositories;

namespace SmartTrinity.App.Migrations.Services
{
    public interface ISalesService
    {
        Task<IEnumerable<Sale>> GetSalesWithStationIdAsync();
    }
    public class SalesService : ISalesService
    {
        private readonly ISalesUnitOfWork _salesUnitOfWork;
        private readonly ICustomerUnitOfWork _customerUnitOfWork;

        public SalesService(ISalesUnitOfWork salesUnitOfWork, ICustomerUnitOfWork customerUnitOfWork)
        {
            _salesUnitOfWork = salesUnitOfWork;
            _customerUnitOfWork = customerUnitOfWork;
        }

        public async Task<IEnumerable<Sale>> GetSalesWithStationIdAsync()
        {
            var customer = _customerUnitOfWork.CustomerRepository.GetAll();
            var sales = await _salesUnitOfWork.SalesRepository.GetAll();
            List<Sale> salesWithCustomerId = new List<Sale>();
            foreach (var sale in sales)
            {
                salesWithCustomerId.Add(new Sale()
                {
                    Id = sale.Id,
                    GradeId = sale.GradeId,
                    EndDate = sale.EndDate,
                    EndTime = sale.EndDate,
                    FinalVolume = sale.FinalVolume,
                    HoseId = sale.HoseId,
                    InitialVolume = sale.InitialVolume,
                    Level = sale.Level,
                    Money = sale.Money,
                    Ppu = sale.Ppu,
                    PresetAmount = sale.PresetAmount,
                    PumpId = sale.PumpId,
                    Saletype = sale.Saletype,
                    StartDate = sale.StartDate,
                    StartTime = sale.StartDate,
                    Volume = sale.Volume,
                    Product = sale.Product,
                    StationId = customer.Id,
                });
            }
            return salesWithCustomerId;
        }
    }
}
