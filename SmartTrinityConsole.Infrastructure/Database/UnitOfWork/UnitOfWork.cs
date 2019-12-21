using System.Data;
using Npgsql;
using SmartTrinityApi.Core.Interfaces.Repository.Repositories;
using SmartTrinityApi.Core.Interfaces.UnitOfWork;
using SmartTrinityConsole.Infrastructure.Database.Repositories;

namespace SmartTrinityConsole.Infrastructure.Database.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        IDbConnection _connection = null;
        IDbTransaction _transaction = null;
        dynamic IUnitOfWork.Connection { get { return _connection; } }
        dynamic IUnitOfWork.Transaction { get { return _transaction; } }
        int TransactionResult = 0;

        // Repositories
        public IConfigValuesRepository ConfigValuesRepository { get; private set; }
        public IGenericConfigValuesRepository GenericConfigValuesRepository { get; private set; }

        public UnitOfWork()
        {
            _connection = new NpgsqlConnection("server=localhost;User ID=postgres;password=ssfpostgres;database=smartshipdb;CommandTimeout=30;Timeout=1000;");
            //HACK: Use DI for inject repositories.
            ConfigValuesRepository = new ConfigValuesRepository(_connection, _transaction);
            GenericConfigValuesRepository = new GenericConfigValuesRepository(_connection, _transaction);
        }

        public int Commit()
        {
            //HACK: Improve this logic.
            int temp = TransactionResult;
            TransactionResult = 0;
            
            _transaction.Commit();
            _connection.Close();
            return temp;
        }

        public void Begin()
        {
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();

            _transaction = null;
        }
    }
}