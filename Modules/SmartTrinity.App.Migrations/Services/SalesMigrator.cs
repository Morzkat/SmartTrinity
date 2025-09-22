using SmartTrinity.App.Migrations.Core;
using SmartTrinity.Shared.Core.Database;
using SmartTrinity.App.FuelStation.Core.Models;
using SmartTrinity.App.Migrations.Core.Services;
using SmartTrinity.App.FuelStation.Core.Database;
using SmartTrinity.App.Payments.Core.Database.Repositories;

namespace SmartTrinity.App.Migrations.Services
{
    public class SalesMigrator : ISalesMigrator
    {
        private readonly ISalesUnitOfWork _salesUnitOfWork;
        private readonly ISalesMigratorUnitOfWork _salesMigratorUnitOfWork;
        private readonly IStationUnitOfWork _stationUnitOfWork;
        private readonly IPaymentRepository _paymentRepository;

        public SalesMigrator(ISalesUnitOfWork salesUnitOfWork, ISalesMigratorUnitOfWork salesMigratorUnitOfWork, IStationUnitOfWork stationUnitOfWork, IPaymentRepository paymentRepository)
        {
            _salesUnitOfWork = salesUnitOfWork;
            _stationUnitOfWork = stationUnitOfWork;
            _paymentRepository = paymentRepository;
            _salesMigratorUnitOfWork = salesMigratorUnitOfWork;
        }

        public Task MigrateSales()
        {
            throw new NotImplementedException();
        }

        public async Task MigrateSalesToCentral()
        {
            var station = await GetDestinationStation();
            var lastSaleId = await _salesMigratorUnitOfWork.SalesMigratorRepository.GetLastSaleId();
            var sales = await _salesUnitOfWork.SalesRepository.GetSalesFromId(lastSaleId);

            var addSalesToCentralServerTasks = new List<Task>();

            foreach (var sale in sales)
            {
                sale.StartDate = sale.StartDate.FormatDateForTable();
                sale.EndDate = sale.EndDate.FormatDateForTable();
                addSalesToCentralServerTasks.Add(_salesMigratorUnitOfWork.SalesMigratorRepository.AddToCentralServer((int)station.Id, sale));
            }

            await Task.WhenAll(addSalesToCentralServerTasks);
        }

        public async Task MigratePaymentsToCentral()
        {
            var station = await GetDestinationStation();
            var lastPaymentId = await _salesMigratorUnitOfWork.PaymentRepository.GetLastPaymentId();
            var payments = await _paymentRepository.GetPaymentsFromId(lastPaymentId);

            // _salesMigratorUnitOfWork.PaymentRepository.Add
        }

        private async Task<Station> GetDestinationStation()
        {
            var currentStation = await _stationUnitOfWork.StationRepository.GetStation();
            var station = await _salesMigratorUnitOfWork.StationRepository.GetStationByRnc(currentStation.Rnc);

            return station;
        }
    }

    //Note: Should this logic be move to shared lib???
    public static class StringExtensions
    {
        public static string FormatDate(this string input)
        {
            return "";
        }

        public static string FormatDateForTable(this string input)
        {
            var values = input.Split('/');
            return $"{values[2]}{values[1]}{values[0]}";
        }
    }

}
