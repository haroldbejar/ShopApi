using System.Security.Cryptography;
using System.Text;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace ShopApi.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="tokenService"></param>
        public UserController(IUserService service, ITokenService tokenService)
        {
            _service = service;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="registerUser"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDTO registerUser) 
        {
            var existingUser = await _service.ValidatUserExist(registerUser.UserName);
            if (existingUser) return BadRequest("User not found!");
            
            await _service.Register(registerUser);

            var user = new LogedUserDTO
            { 
                UserName = registerUser.UserName,
                Token = _tokenService.CreateToken(registerUser)
            };

            return Ok(user);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="registerUser"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult> Login(RegisterDTO registerUser) 
        {
            var existingUser = await _service.GetUserByUserName(registerUser.UserName);
            if (existingUser == null) return Unauthorized();

            using var hmac = new HMACSHA512(existingUser.PasswordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUser.Password));

            for (int i = 0; i < computeHash.Length; i++) 
            {
                if (computeHash[i] != existingUser.PasswordHash[i]) return Unauthorized();
            }

            var user = new LogedUserDTO
            {
                Id = registerUser.Id,
                UserName = registerUser.UserName,
                Token = _tokenService.CreateToken(registerUser)
            };

            return Ok(user);
        }
    }
}