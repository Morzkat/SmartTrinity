using SmartTrinity.Core.Models;
using Microsoft.Extensions.Options;
using SmartTrinity.App.Pumps.Core.Database;
using SmartTrinity.App.Pumps.Core.Database.Repositories;
using SmartTrinity.Infrastructure.Database.UnitOfWork;
using SmartTrinity.App.Pumps.Infrastructure.Repositories;

namespace SmartTrinity.App.Pumps.Infrastructure
{
    public class PumpsUnitOfWork : UnitOfWork, IPumpsUnitOfWork
    {
        private readonly AppSettings _appSettings;

        public IPumpsRepository PumpsRepository { get; private set; }

        public IGenericConfigValuesRepository GenericConfigValuesRepository { get; private set; }

        public PumpsUnitOfWork(IOptions<AppSettings> options) : base(options)
        {
            _appSettings = options.Value;
            PumpsRepository = new PumpsRepository(_connection, _transaction, options);
            GenericConfigValuesRepository = new GenericConfigValuesRepository(_connection, _transaction);
        }
    }
}
