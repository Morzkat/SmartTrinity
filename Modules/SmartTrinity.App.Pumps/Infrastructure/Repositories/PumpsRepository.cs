using System.Data;
using SmartTrinity.Core.Models;
using Microsoft.Extensions.Options;
using SmartTrinity.App.Pumps.Core.Models;
using SmartTrinity.App.Pumps.Core.Database.Repositories;
using SmartTrinity.Shared.Infrastructure.Database.Repositories;

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
            tableName = "SSF_PUMP_SALES";
            Connection = connection;
            Transaction = transaction;
            _appSettings = appSettings.Value;
        }

    }
}
