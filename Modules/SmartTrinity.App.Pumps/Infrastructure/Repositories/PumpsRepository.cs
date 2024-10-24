using Microsoft.Extensions.Options;
using SmartTrinity.App.Pumps.Core.Models;
using SmartTrinity.App.Pumps.Core.Database.Repositories;
using SmartTrinity.Core.Models;
using SmartTrinity.Infrastructure.Database.Repositories;
using System.Data;

namespace SmartTrinity.App.Pumps.Infrastructure.Repositories
{
    public class PumpsRepository : Repository<Pump>, IPumpsRepository
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; }
        public AppSettings _appSettings { get; }

        public PumpsRepository(IDbConnection connection, IDbTransaction transaction, IOptions<AppSettings> appSettings) : base(connection, transaction)
        {
            tableId = "sale_id";
            //TODO: Validate the table name
            tableName = "SSF_PUMP_SALES";
            Connection = connection;
            Transaction = transaction;
            _appSettings = appSettings.Value;
        }

    }
}
