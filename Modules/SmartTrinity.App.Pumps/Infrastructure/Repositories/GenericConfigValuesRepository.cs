using Dapper;
using SmartTrinity.App.Pumps.Core.Models;
using SmartTrinity.App.Pumps.Core.Repositories;
using SmartTrinity.Infrastructure.Database.Repositories;
using System.Data;

namespace SmartTrinity.App.Pumps.Infrastructure.Repositories
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

        public override async Task<GenericConfigValues> Get(int id)
        {
            var entity = await SqlMapper.QueryFirstOrDefaultAsync<GenericConfigValues>(_dbSet, $"SELECT * FROM ssf_generic_config_values WHERE {tableId} = @id", new { id }, _transaction);
            return entity ?? new GenericConfigValues();
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
            GenericConfigValues configValues = await Get(serviceMode.Id);
            configValues.ParamValue = serviceMode.ServiceModeType.ToString();
            return await Update(configValues);
        }
    }
}
