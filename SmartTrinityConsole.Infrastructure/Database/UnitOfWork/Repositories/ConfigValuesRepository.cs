using Dapper;
using System.Data;
using System.Collections.Generic;
using SmartTrinityConsole.Core.Entities.Database.Configs;
using SmartTrinityApi.Core.Interfaces.Repository.Repositories;

namespace SmartTrinityConsole.Infrastructure.Database.Repositories
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

        public IEnumerable<PumpServiceMode> GetPumpsAndServicesModes()
        {
            IEnumerable<PumpServiceMode> entities = new List<PumpServiceMode>();
            entities = SqlMapper.Query<PumpServiceMode>((IDbConnection)_dbSet, $"SELECT sgcv.id_d as id, cv.id as pump, sgcv.param_value as service_mode FROM config_values cv INNER JOIN ssf_generic_config_values sgcv ON cv.id = sgcv.device_id WHERE cv.library = 'pump' AND cv.parameter = 'ID' AND sgcv.parameter = 'authorization' ORDER BY cv.param_value").AsList();

            return entities;
        }

        public void UpdatePumpServiceMode()
        {
            throw new System.NotImplementedException();
        }
    }
}