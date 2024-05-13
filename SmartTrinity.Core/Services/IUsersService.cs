using SmartTrinity.Core.DTOs;
using SmartTrinity.Core.Models;

namespace SmartTrinity.Core.Services
{
    public interface IUsersService
    {
        public Task<User?> GetById(int? userId);
        public Task<UserDTO> Authenticate (UserLoginDTO userToLogin);
    }
}