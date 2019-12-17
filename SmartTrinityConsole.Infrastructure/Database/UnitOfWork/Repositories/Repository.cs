using System;
using Npgsql;
using System.Linq.Expressions;
using System.Collections.Generic;
using SmartTrinityApi.Core.Interfaces.Repository;
using System.Data;
using Dapper;

namespace SmartTrinityConsole.Infrastructure.Database.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected virtual string tableName { get; set; }
        protected virtual string tableId { get; set; }

        protected IDbConnection Connection { get { return new NpgsqlConnection("server=localhost;User ID=postgres;password=ssfpostgres;database=smartshipdb;CommandTimeout=30;Timeout=1000;"); } }
        public void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(int id)
        {
            TEntity entity = null;
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                // dbConnection.Insert(null);
                entity = dbConnection.QueryFirstOrDefault<TEntity>($"SELECT * FROM {tableName} WHERE {tableId} = @Id", new { Id = id });
            }
            return entity;
        }

        public IEnumerable<TEntity> GetAll()
        {
            List<TEntity> entities = new List<TEntity>();

            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                entities = dbConnection.Query<TEntity>($"SELECT * FROM {tableName}").AsList();
            }

            return entities;
        }

        public void Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}