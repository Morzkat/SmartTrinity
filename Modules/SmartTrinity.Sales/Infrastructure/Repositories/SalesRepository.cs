using Dapper;
using System.Data;
using System.Text;
using SmartTrinity.Core.Models;
using  SmartTrinity.App.Sales.Core.Repositories;
using SmartTrinity.Infrastructure.Database.Repositories;
using Microsoft.Extensions.Options;
using  SmartTrinity.App.Sales.Core.Models;

namespace  SmartTrinity.App.Sales.Infrastructure.Repositories
{
    public class SalesRepository : Repository<Sale>, ISalesRepository
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; }
        public AppSettings _appSettings { get; }

        public SalesRepository(IDbConnection connection, IDbTransaction transaction, IOptions<AppSettings> appSettings) : base(connection, transaction)
        {
            tableId = "sale_id";
            tableName = "SSF_PUMP_SALES";
            Connection = connection;
            Transaction = transaction;
            _appSettings = appSettings.Value;
        }

        public override async Task<IEnumerable<Sale>> GetAll()
        {
            var parameters = new { limit = _appSettings.TrinitySettings.SalesLimitPerRequest };
            StringBuilder query = new StringBuilder("SELECT sales.sale_id as Id, sales.pump_id, sales.hose_id, sales.grade_id, ")
            .Append("sales.volume, sales.money, sales.ppu, sales.initial_volume, sales.final_volume, sales.start_date, sales.start_time, ")
            .Append("sales.preset_amount, sales.start_time, sales.end_time, sales.level, sales.sale_type, ")
            .Append("REPLACE(TRIM(BOTH '\n' FROM manguera.producto), '\n', '') as product, ")
            .Append("TO_CHAR(sales.start_date::DATE, 'DD/MM/YYYY') as StartDate, ")
            .Append("TO_CHAR(sales.end_date::DATE, 'DD/MM/YYYY') as EndDate ")
            .Append($"FROM {tableName} sales ")
            .Append("INNER JOIN SSF_LADO_MANGUERA manguera ON ")
            .Append("sales.pump_id = manguera.pump_id AND sales.hose_id = manguera.hose_id ")
            .Append("ORDER BY sales.sale_id DESC LIMIT @limit ")
            ;
            var sales = await SqlMapper.QueryAsync<Sale>(_dbSet, query.ToString(), parameters, transaction: _transaction);
            return sales;
        }
    }
}

