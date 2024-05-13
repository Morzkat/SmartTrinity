using System.Data;
using System.Linq.Expressions;
using Dapper.Contrib.Extensions;
using SmartTrinity.Core.Database;
using SmartTrinity.Core.Database.Models;

namespace SmartTrinity.Infrastructure.Database.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected dynamic _dbSet;
        protected dynamic _transaction;
        protected virtual string tableId { get; set; } = "";
        protected virtual string tableName { get; set; } = "";

        public Repository(dynamic dbSet, dynamic transaction)
        {
            _dbSet = dbSet;
            _transaction = transaction;
        }

        public virtual async Task<int> Add(TEntity entity)
        {
            return await SqlMapperExtensions.InsertAsync((IDbConnection)_dbSet, entity, (IDbTransaction)_transaction);
        }

        public virtual Task<int> AddRange(IEnumerable<TEntity> entities)
        {
            return SqlMapperExtensions.InsertAsync((IDbConnection)_dbSet, entities, (IDbTransaction)_transaction);
        }

        public virtual Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> Get(int id)
        {
            var entity = await SqlMapperExtensions.GetAsync<TEntity>((IDbConnection)_dbSet, id, (IDbTransaction)_transaction);
            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {

            var entities = await SqlMapperExtensions.GetAllAsync<TEntity>((IDbConnection)_dbSet, (IDbTransaction)_transaction);
            return entities;
        }

        public virtual async Task<bool> Remove(TEntity entity)
        {
            return await SqlMapperExtensions.DeleteAsync((IDbConnection)_dbSet, new { tableId = entity.Id }, (IDbTransaction)_transaction);
        }

        public virtual async Task<bool> RemoveRange(IEnumerable<TEntity> entities)
        {
            return await SqlMapperExtensions.DeleteAsync((IDbConnection)_dbSet, entities, (IDbTransaction)_transaction);
        }

        public virtual Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            // SqlMapperExtensions.Delete(entities);
            throw new NotImplementedException();
        }

        public virtual async Task<bool> Update(TEntity entity)
        {
            return await SqlMapperExtensions.UpdateAsync((IDbConnection)_dbSet, entity, (IDbTransaction)_transaction);
        }

        public virtual async Task<bool> UpdateRange(IEnumerable<TEntity> entities)
        {
            return await SqlMapperExtensions.UpdateAsync((IDbConnection)_dbSet, entities, (IDbTransaction)_transaction);
        }
    }
}