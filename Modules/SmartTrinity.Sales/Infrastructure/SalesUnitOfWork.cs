using Microsoft.Extensions.Options;
using SmartTrinity.Core.Database;
using SmartTrinity.Core.Models;
using  SmartTrinity.App.Sales.Core.Repositories;
using SmartTrinity.Infrastructure.Database.UnitOfWork;

namespace  SmartTrinity.App.Sales.Infrastructure.Repositories
{
    public interface ISalesUnitOfWork : IUnitOfWork
    {
        ISalesRepository SalesRepository { get; }
    }
    public sealed class SalesUnitOfWork : UnitOfWork, ISalesUnitOfWork
    {
        private readonly AppSettings _appSettings;

        public ISalesRepository SalesRepository { get; private set; }

        public SalesUnitOfWork(IOptions<AppSettings> options) : base(options)
        {
            _appSettings = options.Value;
            SalesRepository = new SalesRepository(_connection, _transaction, options);
        }
    }
}