using System;
using System.Data;
using SmartTrinityApi.Core.Interfaces.Repository.Repositories;

namespace SmartTrinityApi.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IConfigValuesRepository ConfigValuesRepository { get; }
        IGenericConfigValuesRepository GenericConfigValuesRepository { get; }
        dynamic Connection { get; }
        dynamic Transaction { get; }
        int Commit();
        void Begin();
        void Rollback();
    }
}