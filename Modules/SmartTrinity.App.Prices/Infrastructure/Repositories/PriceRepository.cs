using Dapper;
using System.Data;
using System.Text;
using SmartTrinity.Core.Models;
using Microsoft.Extensions.Options;
using SmartTrinity.App.Prices.Core.Models;
using SmartTrinity.App.Prices.Core.Database.Repositories;
using SmartTrinity.Shared.Infrastructure.Database.Repositories;

namespace SmartTrinity.App.Prices.Infrastructure.Repositories
{
    public class PriceRepository : Repository<Price>, IPricesRepository
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; }
        public AppSettings _appSettings { get; }

        public PriceRepository(IDbConnection connection, IDbTransaction transaction, IOptions<AppSettings> appSettings) : base(connection, transaction)
        {
            tableId = "price_change_id";
            tableName = "SSF_ADDIN_PRICES_CHANGE";
            Connection = connection;
            Transaction = transaction;
            _appSettings = appSettings.Value;
        }

        public async Task<IList<Price>> GetPrices(int offset, int limit)
        {
            var parameters = new { limit, offset };
            StringBuilder query = new StringBuilder()
            .Append("SELECT price_change_id, application_date, application_time, processed_date, processed_time")
            .Append("FROM ssf_addin_prices_change t ORDER BY t.price_change_id ")
            .Append("DESC LIMIT @limit offset @offset");

            var entities = await SqlMapper.QueryAsync<Price>(_dbSet, query.ToString(), parameters);
            return entities;
        }

        public async override Task<int> Add(Price entity)
        {
            var query = $"INSERT INTO {tableName} (price_change_id,application_date,application_time) VALUES (@Id, @ApplicationDate, @ApplicationTime)";
            return await SqlMapper.ExecuteAsync(_dbSet, query, entity);
        }

        public async Task<int> GetNextPriceChangeId()
        {
            var query = $"SELECT MAX(price_change_id)+1 as next_id FROM ${tableName}";
            return await SqlMapper.QueryAsync<int>(_dbSet, query) ?? 1;
        }

    }
}
