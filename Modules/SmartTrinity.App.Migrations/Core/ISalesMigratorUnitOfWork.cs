using SmartTrinity.App.Migrations.Core.Repositories;
using SmartTrinity.App.FuelStation.Core.Database.Repositories;
using SmartTrinity.App.Payments.Core.Database.Repositories;

namespace SmartTrinity.App.Migrations.Core
{
    public interface ISalesMigratorUnitOfWork
    {
        IStationRepository StationRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        ISalesMigratorRepository SalesMigratorRepository { get; }
    }
}
