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
    public class PriceDetailsRepository: Repository<PriceDetails>, IPriceDetailsRepository
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; }
        public AppSettings _appSettings { get; }

        public PriceDetailsRepository(IDbConnection connection, IDbTransaction transaction, IOptions<AppSettings> appSettings) : base(connection, transaction)
        {
            tableId = "price_change_id";
            tableName = "SSF_ADDIN_PRICES_CHANGE_DETAIL";
            Connection = connection;
            Transaction = transaction;
            _appSettings = appSettings.Value;
        }

        public async Task<IList<PriceDetails>> GetPriceDetails(int priceId)
        {
            var parameters = new { priceId };
            StringBuilder query = new StringBuilder()
           .Append("SELECT t.*,(SELECT tkt_plu_long_desc FROM ssf_tkt_plu WHERE cast(tkt_plu_id as integer)=t.grade_id) as product ")
           .Append(" FROM ssf_addin_prices_change_detail t WHERE t.price_change_id=@priceId ");

            var entities = await SqlMapper.QueryAsync<PriceDetails>(_dbSet, query.ToString());
            return entities;
        }

        public async override Task<int> Add(PriceDetails entity)
        {
            var query = $"INSERT INTO ${tableName} (price_change_id, grade_id, price_level, ppu) VALUES (@Id, @GradeId, @PriceLevel, @Ppu)";
            return await SqlMapper.ExecuteAsync(_dbSet, query, entity);
        }
    }
}
