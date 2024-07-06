using Domain.DTOs;

namespace Service.Services
{
    public interface ITokenService
    {
        string CreateToken(RegisterDTO user);
    }
}