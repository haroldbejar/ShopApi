using System.Security.Cryptography;
using System.Text;
using Domain.DTOs;
using Domain.Entities;
using Repository.Repos;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly IUserRepository _userRepository;

        public UserService(
            IRepository<User> repository, 
            IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public Task<UserDTO> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> GetUserByUserName(string userName)
        {
           var user = await _userRepository.GetUserByUserName(userName);
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt
            };
        }

        public async Task<UserDTO> Register(RegisterDTO registerDTO)
        {
            using var hmac = new HMACSHA512();

            var user = new User
            {
                UserName = registerDTO.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };

            await _repository.InsertAsync(user);

            return new UserDTO
            {
                UserName = registerDTO?.UserName
            };
        }

        public async Task<bool> ValidatUserExist(string userName)
        {
             var existingUser = await _userRepository.GetUserByUserName(userName);
            if (existingUser != null) return true;

            return false;
        }
    }
}