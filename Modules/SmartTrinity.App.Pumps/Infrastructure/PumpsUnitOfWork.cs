using Microsoft.Extensions.Options;
using SmartTrinity.App.Pumps.Core.Repositories;
using SmartTrinity.App.Pumps.Infrastructure.Repositories;
using SmartTrinity.Core.Database;
using SmartTrinity.Core.Models;
using SmartTrinity.Infrastructure.Database.UnitOfWork;

namespace SmartTrinity.App.Pumps.Infrastructure
{
    public interface IPumpsUnitOfWork : IUnitOfWork
    {
        IPumpsRepository PumpsRepository { get; }
        IGenericConfigValuesRepository GenericConfigValuesRepository { get; }
    }

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
