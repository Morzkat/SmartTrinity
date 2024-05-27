using Dapper;
using Microsoft.Extensions.Options;
using SmartTrinity.App.Migrations.Core.Models;
using SmartTrinity.App.Migrations.Core.Repositories;
using SmartTrinity.Core.Models;
using SmartTrinity.Infrastructure.Database.Repositories;
using System.Data;
using System.Text;

namespace SmartTrinity.App.Migrations.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; }
        public AppSettings _appSettings { get; }

        public CustomerRepository(IDbConnection connection, IDbTransaction transaction, IOptions<AppSettings> appSettings) : base(connection, transaction)
        {
            tableId = "id";
            tableName = "ssf_datos_cliente";
            Connection = connection;
            Transaction = transaction;
            _appSettings = appSettings.Value;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            var parameters = new { limit = _appSettings.TrinitySettings.SalesLimitPerRequest };
            StringBuilder query = new StringBuilder($"SELECT id FROM {tableName}");
            var customerId = await SqlMapper.QueryAsync<int>(_dbSet, query.ToString(), parameters, transaction: _transaction);
            return new List<Customer>() { new Customer() { Id = customerId } };
        }

    }
}
