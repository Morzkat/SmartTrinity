using Dapper;
using System.Data;
using System.Text;
using SmartTrinity.Core.Models;
using Microsoft.Extensions.Options;
using SmartTrinity.Shared.Core.Models;
using SmartTrinity.App.Sales.Core.Repositories;
using SmartTrinity.Shared.Infrastructure.Database.Dapper.UnitOfWork.Repositories;

namespace SmartTrinity.App.Sales.Infrastructure.Repositories
{
    public class AppSalesRepository : SalesRepository, IAppSalesRepository
    {

        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; }
        public AppSettings _appSettings { get; }

        public AppSalesRepository(IDbConnection connection, IDbTransaction transaction, IOptions<AppSettings> appSettings) : base(connection, transaction, appSettings)
        {
            tableId = "sale_id";
            tableName = "SSF_PUMP_SALES";
            Connection = connection;
            Transaction = transaction;
            _appSettings = appSettings.Value;
        }

        public async Task<int> AddToCentralServer(int stationId, Sale sale)
        {
            var query = new StringBuilder($"INSERT INTO ${tableName} (sale_id, pump_id, hose_id, grade_id, money, volume, ppu, level, sale_type, initial_volume, final_volume, ")
                .Append(",,,, preset_amount, sale_auth)");

            var p = await SqlMapper.ExecuteAsync(_dbSet, query.ToString(), transaction: _transaction);
            throw new NotImplementedException();
        }
    }
}
