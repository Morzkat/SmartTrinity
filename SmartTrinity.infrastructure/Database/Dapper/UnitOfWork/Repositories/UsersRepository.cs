using Dapper;
using System.Data;
using System.Text;
using SmartTrinity.Core.Models;
using SmartTrinity.Core.Database.Repositories;
using SmartTrinity.Infrastructure.Database.Repositories;
using SmartTrinity.Core.DTOs;

namespace SmartTrinity.Infrastructure.Database
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {

        public UsersRepository(IDbConnection connection, IDbTransaction transaction) : base(connection, transaction)
        {
            tableId = "Id";
            tableName = "SSF_USERS";
        }

        public override async Task<User> Get(int id)
        {
            var parameters = new { id };
            var query = $"SELECT * FROM {tableName} WHERE Id = @id";
            return await SqlMapper.QueryFirstOrDefaultAsync<User>(_dbSet, query, parameters, transaction: _transaction);
        }

        public async Task<User> GetByUsernameAndPassword(UserLoginDTO user)
        {
            var parameters = new { user.Username, user.Password };
            var query = $"SELECT * FROM {tableName} WHERE Username = @userName AND Password = @password";
            return await SqlMapper.QueryFirstOrDefaultAsync<User>(_dbSet, query, parameters, transaction: _transaction);
        }
    }
}