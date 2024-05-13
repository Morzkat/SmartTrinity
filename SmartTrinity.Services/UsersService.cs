using SmartTrinity.Core.Services;
using SmartTrinity.Core.Models;
using SmartTrinity.Core.Database;
using SmartTrinity.Core.DTOs;
using Microsoft.Extensions.Options;

namespace SmartTrinity.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public AppSettings _appSettings { get; private set; }

        public UsersService(IUnitOfWork unitOfWork, IJwtService jwtService, IOptions<AppSettings> appSettings)
        {
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }

        public async Task<User?> GetById(int? userId)
        {
            if (userId == null)
                throw new Exception("Id cannot be null");

            return await _unitOfWork.UsersRepository.Get(userId.Value);
        }

        public async Task<UserDTO> Authenticate(UserLoginDTO userToLogin)
        {
            var user = await _unitOfWork.UsersRepository.GetByUsernameAndPassword(userToLogin) ?? throw new Exception("User is not register, validate credentials");
            var token = await  _jwtService.GenerateJwtToken(user);

            return new UserDTO { Username = user.Username, Token = $"bearer {token}" };
        }
    }
}
