using SmartTrinity.App.Migrations.Core.Repositories;
using SmartTrinity.App.FuelStation.Core.Database.Repositories;

namespace SmartTrinity.App.Migrations.Core
{
    public interface ISalesMigratorUnitOfWork
    {
        IStationRepository StationRepository { get; }
        ISalesMigratorRepository SalesMigratorRepository { get; }
    }
}
