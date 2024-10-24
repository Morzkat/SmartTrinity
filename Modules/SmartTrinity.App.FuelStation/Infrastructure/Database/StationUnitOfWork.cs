


using Microsoft.Extensions.Options;
using SmartTrinity.App.FuelStation.Core.Database;
using SmartTrinity.App.FuelStation.Core.Database.Repositories;
using SmartTrinity.App.FuelStation.Infrastructure.Repositories;
using SmartTrinity.Core.Database;
using SmartTrinity.Core.Models;
using SmartTrinity.Infrastructure.Database.UnitOfWork;

namespace SmartTrinity.App.FuelStation.Infrastructure
{

    public class StationUnitOfWork : UnitOfWork, IStationUnitOfWork
    {
        public IStationRepository StationRepository { get; private set; }

        public StationUnitOfWork(IOptions<AppSettings> opitons) : base(opitons)
        {
            StationRepository = new StationRepository(_connection, _transaction);
        }
    }
}