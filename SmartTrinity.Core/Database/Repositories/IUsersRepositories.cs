using SmartTrinity.Core.DTOs;
using SmartTrinity.Core.Models;

namespace SmartTrinity.Core.Database.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        public Task<User> GetByUsernameAndPassword(UserLoginDTO user);
    }
}