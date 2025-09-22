using Npgsql;
using System.Data;
using SmartTrinity.Core.Models;
using Microsoft.Extensions.Options;
using SmartTrinity.App.Migrations.Core;
using SmartTrinity.App.Migrations.Core.Models;
using SmartTrinity.App.Migrations.Core.Repositories;
using SmartTrinity.Infrastructure.Database.UnitOfWork;
using SmartTrinity.App.Migrations.Infrastructure.Repositories;
using SmartTrinity.App.FuelStation.Core.Database.Repositories;
using SmartTrinity.App.FuelStation.Infrastructure.Repositories;
using SmartTrinity.App.Payments.Core.Database.Repositories;

namespace SmartTrinity.App.Migrations.Infrastructure
{
    public class SalesMigratorUnitOfWork : UnitOfWork, ISalesMigratorUnitOfWork
    {
        protected IDbConnection _connection;
        protected IDbTransaction _transaction;
        private readonly AppSettings _appSettings;

        public IStationRepository StationRepository { get; private set; }
        public ISalesMigratorRepository SalesMigratorRepository { get; private set; }

        public IPaymentRepository PaymentRepository { get; private set; }

        public SalesMigratorUnitOfWork(IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _appSettings = appSettings.Value;
            _connection = new NpgsqlConnection(_appSettings.TrinitySettings.TrinityConnectionString);
        }

        public SalesMigratorUnitOfWork(IOptions<AppSettings> appSettings, IOptions<MigratorSettings> migratorSettingsOptions) : base(appSettings)
        {
            var migratorSettings = migratorSettingsOptions.Value;
            StationRepository = new StationRepository(new NpgsqlConnection(migratorSettings.SalesConnectionString), _transaction);
            SalesMigratorRepository = new SalesMigratorRepository(new NpgsqlConnection(migratorSettings.SalesConnectionString), _transaction, appSettings);
        }
    }
}

