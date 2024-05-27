using Microsoft.Extensions.Options;
using SmartTrinity.App.Migrations.Core.Repositories;
using SmartTrinity.App.Migrations.Infrastructure.Repositories;
using SmartTrinity.Core.Database;
using SmartTrinity.Core.Models;
using SmartTrinity.Infrastructure.Database.UnitOfWork;

namespace SmartTrinity.App.Migrations.Infrastructure
{
    public interface ICustomerUnitOfWork : IUnitOfWork
    {
        ICustomerRepository CustomerRepository { get; }
    }
    public sealed class CustomerUnitOfWork : UnitOfWork, ICustomerUnitOfWork
    {
        private readonly AppSettings _appSettings;

        public ICustomerRepository CustomerRepository { get; private set; }

        public CustomerUnitOfWork(IOptions<AppSettings> options) : base(options)
        {
            _appSettings = options.Value;
            CustomerRepository = new CustomerRepository(_connection, _transaction, options);
        }
    }
}
