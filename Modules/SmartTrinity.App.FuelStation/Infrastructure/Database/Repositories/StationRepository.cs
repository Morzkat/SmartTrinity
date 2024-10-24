using System.Data;
using SmartTrinity.Core.Models;
using SmartTrinity.App.FuelStation.Core.Models;
using SmartTrinity.Infrastructure.Database.Repositories;
using SmartTrinity.App.FuelStation.Core.Database.Repositories;
using Dapper;

namespace SmartTrinity.App.FuelStation.Infrastructure.Repositories
{
    public class StationRepository : Repository<Station>, IStationRepository
    {

        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; }
        public AppSettings _appSettings { get; }

        public StationRepository(IDbConnection connection, IDbTransaction transaction) : base(connection, transaction)
        {
            tableId = "id";
            tableName = "SSF_DATOS_CLIENTE";
            Connection = connection;
            Transaction = transaction;
        }

        public async Task<Station> GetStation()
        {
            var entity = await SqlMapper.QueryFirstOrDefaultAsync<Station>(_dbSet, $"SELECT id, rnc, name, telefono as telephone, address FROM {tableName} LIMIT 1;", transaction: _transaction);
            return entity ?? new Station();
        }

        public async Task<Station> GetStationByRnc(string rnc)
        {
            try
            {
                var entity = await SqlMapper.QueryFirstOrDefaultAsync<Station>(_dbSet,
                $"SELECT id, rnc, name, telefono as telephone, address FROM {tableName} WHERE rnc = {rnc} LIMIT 1;",
                transaction: _transaction);

                return entity ?? new Station();
            }
            catch (Exception ex)
            {

                throw new Exception($"Station with RNC: {rnc} not found in destination database.");
            }
        }
    }
}
