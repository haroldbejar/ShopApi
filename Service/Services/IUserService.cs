using Domain.DTOs;

namespace Service.Services
{
    public interface IUserService
    {
        Task<UserDTO> DeleteAsync(int id);
        Task<UserDTO> GetUserByUserName(string userName);
        Task<UserDTO> Register(RegisterDTO registerDTO);
        Task<bool> ValidatUserExist(string userName);
    }
}