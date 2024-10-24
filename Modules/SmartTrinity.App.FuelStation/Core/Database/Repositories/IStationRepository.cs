using SmartTrinity.Core.Database;
using SmartTrinity.App.FuelStation.Core.Models;

namespace SmartTrinity.App.FuelStation.Core.Database.Repositories
{
    public interface IStationRepository: IRepository<Station>
    {
        Task<Station> GetStation();
        Task<Station> GetStationByRnc(string rnc);
    }
}
