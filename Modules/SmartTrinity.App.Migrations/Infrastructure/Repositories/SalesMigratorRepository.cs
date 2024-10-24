using Dapper;
using Microsoft.Extensions.Options;
using SmartTrinity.App.Migrations.Core.Repositories;
using SmartTrinity.App.Sales.Core.Repositories;
using SmartTrinity.Core.Models;
using System.Data;
using System.Text;
using SmartTrinity.Shared.Core.Models;
using SmartTrinity.Shared.Infrastructure.Database.Dapper.UnitOfWork.Repositories;
using System.Collections.Generic;

namespace SmartTrinity.App.Migrations.Infrastructure.Repositories
{
    public class SalesMigratorRepository : SalesRepository, ISalesMigratorRepository
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; }
        public AppSettings _appSettings { get; }

        public SalesMigratorRepository(IDbConnection connection, IDbTransaction transaction, IOptions<AppSettings> appSettings) : base(connection, transaction, appSettings)
        {
            tableId = "sale_id";
            tableName = "SSF_PUMP_SALES";
            Connection = connection;
            Transaction = transaction;
            _appSettings = appSettings.Value;
        }

        public async Task<int> AddToCentralServer(int stationId, Sale sale)
        {
            var query = new StringBuilder($"INSERT INTO {tableName} (sale_id, pump_id, hose_id, grade_id, money, volume, ppu, level, sale_type, initial_volume, ")
                .Append("final_volume, start_date, start_time, end_date, end_time, preset_amount, sale_auth, station_id)")
                .Append(" VALUES ")
                .Append("(@Id, @PumpId, @HoseId, @GradeId, @Money, @Volume, @Ppu, @Level, @Saletype, @InitialVolume, ")
                .Append($"@FinalVolume, @StartDate, @StartTime, @EndDate, @EndTime, @PresetAmount, '', {stationId})");

            var r = await SqlMapper.ExecuteAsync(_dbSet, query.ToString(), sale, transaction: _transaction);

            return 1;
        }
    }
}
