using System;
using SmartTrinityApi.Core.Interfaces.Repository.Repositories;

namespace SmartTrinityApi.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IConfigValuesRepository ConfigValuesRepository { get; }
        IGenericConfigValuesRepository GenericConfigValuesRepository { get; }
        
        int Commit();
    }
}