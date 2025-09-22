using SmartTrinity.App.FuelStation.Core.Database.Repositories;
using SmartTrinity.Core.Database;

namespace SmartTrinity.App.FuelStation.Core.Database
{
    public interface IStationUnitOfWork : IUnitOfWork
    {
        IStationRepository StationRepository { get; }
    }
}
