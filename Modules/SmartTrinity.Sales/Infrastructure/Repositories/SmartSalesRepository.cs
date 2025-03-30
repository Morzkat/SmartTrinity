using Dapper;
using System.Data;
using System.Text;
using SmartTrinity.Core.Models;
using Microsoft.Extensions.Options;
using SmartTrinity.Shared.Core.Models;
using SmartTrinity.App.Sales.Core.Models.Filters;
using SmartTrinity.Shared.Infrastructure.Database.Dapper.UnitOfWork.Repositories;

namespace SmartTrinity.App.Sales.Infrastructure.Repositories
{
    public interface ISmartSalesRepository
    {
        Task<IEnumerable<Sale>> GetSales(SaleQueryFilter queryFilters);
    }

    public class SmartSalesRepository : SalesRepository, ISmartSalesRepository
    {
        public SmartSalesRepository(IDbConnection connection, IDbTransaction transaction, IOptions<AppSettings> appSettings) : base(connection, transaction, appSettings)
        {
            this.tableName = "ssf_pump_sales";
        }

        public async Task<IEnumerable<Sale>> GetSales(SaleQueryFilter queryFilters)
        {
            var filters = new StringBuilder();
            object param = new
            {
                SaleId = queryFilters?.SaleId,
                StartDate = queryFilters?.StartDate.Value.ToString("dd/MM/yyyy"),
                EndDate = queryFilters?.EndDate.Value.ToString("dd/MM/yyyy"),
                PumpId = queryFilters?.PumpId,
                HoseId = queryFilters?.HoseId,
                limit = _appSettings.TrinitySettings.SalesLimitPerRequest
            };

            if (queryFilters.SaleId.HasValue && queryFilters.SaleId.Value > 0)
            {
                filters.Append($" AND sales.sale_id = @SaleId");
            }

            if (queryFilters.PumpId.HasValue)
                filters.Append($" AND sales.pump_id = @PumpId");

            if (queryFilters.HoseId.HasValue)
                filters.Append($" AND sales.hose_id = @HoseId");

            if (queryFilters.StartDate.HasValue && queryFilters.EndDate.HasValue)
            {
                filters.Append($" AND (sales.start_date::DATE >= to_date(@StartDate, 'DD/MM/YYYY') AND sales.end_date::DATE <= to_date(@EndDate, 'DD/MM/YYYY') )");
            }

            var query = new StringBuilder("SELECT sales.sale_id as Id, sales.pump_id, sales.hose_id, sales.grade_id, ")
                .Append("sales.volume, sales.money, sales.ppu, sales.initial_volume, sales.final_volume, sales.start_date, sales.start_time, ")
                .Append("sales.preset_amount, sales.start_time, sales.end_time, sales.level, sales.sale_type, ")
                .Append("REPLACE(TRIM(BOTH '\n' FROM manguera.producto), '\n', '') as product, ")
                .Append("TO_CHAR(sales.start_date::DATE, 'DD/MM/YYYY') as StartDate, ")
                .Append("TO_CHAR(sales.end_date::DATE, 'DD/MM/YYYY') as EndDate ")
                .Append($"FROM {this.tableName} as sales ")
                .Append("INNER JOIN SSF_LADO_MANGUERA manguera ON ")
                .Append("sales.pump_id = manguera.pump_id AND sales.hose_id = manguera.hose_id ")
                .Append($"WHERE 1=1 {filters} ")
                .Append("ORDER BY sales.sale_id DESC LIMIT @limit ");

            List<Sale> sales = await SqlMapper.QueryAsync<Sale>(_dbSet, query.ToString(), param: param, transaction: _transaction);

            return sales;
        }
    }
}
