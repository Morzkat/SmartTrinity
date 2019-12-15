using System;
using System.Collections.Generic;
using SmartTrinityApi.Core.Entities.Models;

namespace SmartTrinityApi.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        int Commit();
    }
}