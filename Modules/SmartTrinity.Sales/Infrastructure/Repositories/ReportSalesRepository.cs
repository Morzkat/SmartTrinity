using Dapper;
using System.Data;
using System.Text;
using SmartTrinity.Core.Models;
using Microsoft.Extensions.Options;
using SmartTrinity.Shared.Core.Models;
using SmartTrinity.App.Sales.Core.Models;
using SmartTrinity.App.Sales.Core.Repositories;
using SmartTrinity.Infrastructure.Database.Repositories;

namespace SmartTrinity.App.Sales.Infrastructure.Repositories
{
    public class ReportSalesRepository : Repository<ReportSale>, IReportSalesRepository
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; }
        public AppSettings _appSettings { get; }

        public ReportSalesRepository(IDbConnection connection, IDbTransaction transaction, IOptions<AppSettings> appSettings) : base(connection, transaction)
        {
            tableId = "sale_id";
            tableName = "SSF_REPORTES_PUMP_SALES";
            Connection = connection;
            Transaction = transaction;
            _appSettings = appSettings.Value;
        }

        public async Task<int> GetLastId()
        {
            StringBuilder query = new StringBuilder($"SELECT sales.sale_id as Id FROM ssf_report_pump_sale sales ORDER BY sales.sale_id DESC LIMIT 1");

            List<Sale> sales = await SqlMapper.QueryAsync<Sale>(_dbSet, query.ToString(), transaction: _transaction);
            return sales.Count == 0 ? 0 : (int)sales[0].Id;
        }

        public async Task<object> GetReportSalesFromId(int saleId)
        {
            var parameters = new { saleId, limit = 100 };
            StringBuilder query = new StringBuilder("SELECT sales.sale_id as Id, sales.pump_id, sales.hose_id, sales.grade_id, ")
            .Append("sales.volume, sales.money, sales.ppu, sales.initial_volume, sales.final_volume, sales.start_date, sales.start_time, ")
            .Append("sales.start_time, sales.end_time, sales.level, sales.sale_type, sales.producto as producto, sales.sale_auth, ")
            .Append("sales.dia as day, sales.turno as shift")
            .Append("TO_CHAR(sales.start_date::DATE, 'DD/MM/YYYY') as StartDate, TO_CHAR(sales.end_date::DATE, 'DD/MM/YYYY') as EndDate ")
            .Append($"FROM {tableName} sales ")
            .Append("WHERE sales.sale_id > @saleId ")
            .Append("ORDER BY sales.sale_id DESC LIMIT @limit ");

            var sales = await SqlMapper.QueryAsync<Sale>(_dbSet, query.ToString(), parameters, transaction: _transaction);
            return sales;
        }
    }
}
