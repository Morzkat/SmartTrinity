using System.Data;
using SmartTrinityConsole.Core.Entities.Pump;
using SmartTrinityConsole.Core.Entities.Database.Configs;
using SmartTrinityApi.Core.Interfaces.Repository.Repositories;
using Dapper.Contrib;
using Dapper;

namespace SmartTrinityConsole.Infrastructure.Database.Repositories
{
    public class GenericConfigValuesRepository : Repository<GenericConfigValues>, IGenericConfigValuesRepository
    {
        protected override string tableName { get; set; }
        protected override string tableId { get; set; }

        public GenericConfigValuesRepository(IDbConnection dbSet, IDbTransaction transaction) : base(dbSet, transaction)
        {
            tableId = "id_d";
            tableName = "SSF_GENERIC_CONFIG_VALUES";
        }

        public override GenericConfigValues Get(int id)
        {
            GenericConfigValues entity = null;
            entity = SqlMapper.QueryFirstOrDefault<GenericConfigValues>((IDbConnection)_dbSet, $"SELECT * FROM ssf_generic_config_values WHERE {tableId} = @id", new { id = id }, (IDbTransaction)_transaction);
            return entity;
        }

        public override bool Update(GenericConfigValues entity)
        {
            //TODO: Use database transactions for manage table transactions.
            //HACK: Use reflection for get no null properties and add them to the parameters.
            int result = SqlMapper.Execute((IDbConnection)_dbSet, $"UPDATE ssf_generic_config_values SET parameter = @parameter, param_value = @paramValue WHERE {tableId} = @id", entity, (IDbTransaction)_transaction);

            return result > 0 ? true : false;
        }

        public bool UpdatePumpServiceMode(PumpServiceMode pumpServiceMode)
        {
            GenericConfigValues configValues = Get(pumpServiceMode.Id);
            configValues.ParamValue = pumpServiceMode.ServiceMode;
            return Update(configValues);
        }
    }
}
