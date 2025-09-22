using Microsoft.Extensions.Options;
using SmartTrinity.App.Prices.Core.Database;
using SmartTrinity.App.Prices.Core.Database.Repositories;
using SmartTrinity.App.Prices.Infrastructure.Repositories;
using SmartTrinity.Core.Database;
using SmartTrinity.Core.Models;
using SmartTrinity.Infrastructure.Database.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Prices.Infrastructure
{
    public class PriceUnitOfWork : UnitOfWork, IPriceUnitOfWork
    {
        private readonly AppSettings _appSettings;

        public IPricesRepository PricesRepository { get; private set; }

        public IPriceDetailsRepository PricesDetailsRepository { get; private set; }

        public PriceUnitOfWork(IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _appSettings = appSettings.Value;
            PricesRepository = new PriceRepository(_connection, _transaction, appSettings);
        }
    }
}
