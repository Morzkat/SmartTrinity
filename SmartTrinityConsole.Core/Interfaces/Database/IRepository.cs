using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SmartTrinityApi.Core.Entities.Models;

namespace SmartTrinityApi.Core.Interfaces.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        int Add(TEntity entity);

        int AddRange(IEnumerable<TEntity> entities);

        bool Remove(TEntity entity);

        bool RemoveRange(IEnumerable<TEntity> entities);

        bool Update(TEntity entity);

        bool UpdateRange(IEnumerable<TEntity> entities);
    }
}