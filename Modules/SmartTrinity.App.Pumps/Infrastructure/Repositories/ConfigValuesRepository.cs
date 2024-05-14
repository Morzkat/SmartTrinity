using Dapper;
using System.Data;
using SmartTrinity.App.Pumps.Core.Models;
using SmartTrinity.App.Pumps.Core.Repositories;
using SmartTrinity.Infrastructure.Database.Repositories;

namespace SmartTrinity.App.Pumps.Infrastructure.Repositories
{
    public class ConfigValuesRepository : Repository<ConfigValues>, IConfigValuesRepository
    {
        protected override string tableName { get; set; }
        protected override string tableId { get; set; }

        public ConfigValuesRepository(IDbConnection dbSet, IDbTransaction transaction) : base(dbSet, transaction)
        {
            tableName = "CONFIG_VALUES";
            tableId = "id";
        }

        public async Task<IEnumerable<ServiceMode>> GetPumpsAndServicesModes()
        {
            var entities = await SqlMapper.QueryAsync<ServiceMode>(_dbSet, $"SELECT sgcv.id_d as id, cv.id as pump, sgcv.param_value as service_mode FROM config_values cv INNER JOIN ssf_generic_config_values sgcv ON cv.id = sgcv.device_id WHERE cv.library = 'pump' AND cv.parameter = 'ID' AND sgcv.parameter = 'authorization' ORDER BY cv.param_value");
            return entities;
        }
    }
}
