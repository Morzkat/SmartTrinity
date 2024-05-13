using System;
using System.Data;
using SmartTrinity.Core.Database.Repositories;

namespace SmartTrinity.Core.Database
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersRepository UsersRepository { get; }
        int Commit();
        void Begin();
        void Rollback();
    }
}
