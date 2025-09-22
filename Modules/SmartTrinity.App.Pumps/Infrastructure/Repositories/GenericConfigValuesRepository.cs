using Dapper;
using SmartTrinity.App.Pumps.Core.Models;
using System.Data;
using static Dapper.SqlMapper;
using SmartTrinity.Shared.Infrastructure.Database.Repositories;
using SmartTrinity.App.Pumps.Core.Database.Repositories;

namespace SmartTrinity.App.Pumps.Infrastructure.Repositories
{
    public class GenericConfigValuesRepository : Repository<GenericConfigValues>, IGenericConfigValuesRepository
    {
        protected override string tableName { get; set; }
        protected override string tableId { get; set; }

        public GenericConfigValuesRepository(IDbConnection dbSet, IDbTransaction transaction) : base(dbSet, transaction)
        {
            tableId = "id";
            tableName = "SSF_GENERIC_CONFIG_VALUES";
        }

        public override async Task<GenericConfigValues> Get(int id)
        {
            var entity = await SqlMapper.QueryFirstOrDefaultAsync<GenericConfigValues>(_dbSet, $"SELECT * FROM ssf_generic_config_values WHERE {tableId} = @id", new { id }, _transaction);
            return entity ?? new GenericConfigValues();
        }

        public async Task<GenericConfigValues> GetServiceModeByDeviceId(int id)
        {
            return await SqlMapper.QueryFirstOrDefaultAsync<GenericConfigValues>(_dbSet, $"SELECT * FROM ssf_generic_config_values WHERE {tableId} = 'ServiceModes' AND device_id = @id", new { id }, _transaction);
        }

        public override async Task<bool> Update(GenericConfigValues entity)
        {
            //TODO: Use database transactions for manage table transactions.
            //HACK: Use reflection for get no null properties and add them to the parameters.
            int result = await ((IDbConnection)_dbSet).ExecuteAsync($"UPDATE ssf_generic_config_values SET parameter = @parameter, param_value = @paramValue WHERE {tableId} = @id", entity, (IDbTransaction)_transaction);
            return result > 0 ? true : false;
        }

        public async Task<bool> UpdatePumpServiceMode(ServiceMode serviceMode)
        {
            GenericConfigValues configValues = await GetServiceModeByDeviceId(serviceMode.PumpNo);

            if (configValues == null)
                throw new Exception($"Pump with id:{serviceMode.PumpNo} not found, validate pump id.");

            configValues.ParamValue = serviceMode.ServiceModeType.ToString();
            
            int result = await ((IDbConnection)_dbSet).ExecuteAsync($"UPDATE ssf_generic_config_values SET parameter = @parameter, param_value = @paramValue WHERE {tableId} = 'ServiceModes' AND device_id = @deviceId", configValues, (IDbTransaction)_transaction);
            return result > 0 ? true : false;
        }
    }
}
