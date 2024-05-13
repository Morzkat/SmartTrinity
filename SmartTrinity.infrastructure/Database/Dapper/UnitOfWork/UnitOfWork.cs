using System.Data;
using Microsoft.Extensions.Options;
using Npgsql;
using SmartTrinity.Core.Database;
using SmartTrinity.Core.Database.Repositories;
using SmartTrinity.Core.Models;

namespace SmartTrinity.Infrastructure.Database.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        protected IDbConnection _connection;
        protected IDbTransaction _transaction;
        int TransactionResult = 0;
        private readonly AppSettings _appSettings;

        public IUsersRepository UsersRepository { get; private set; }

        // public UnitOfWork(string connectionString)
        // {
        //     _connection = new NpgsqlConnection(connectionString);
        //     UsersRepository = new UsersRepository(_connection, _transaction);
        // }

        public UnitOfWork(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _connection = new NpgsqlConnection(_appSettings.TrinitySettings.TrinityConnectionString);
            UsersRepository = new UsersRepository(_connection, _transaction);
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