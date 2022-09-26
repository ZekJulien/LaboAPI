using DAL.Exceptions;
using DAL.Interfaces;
using LaboAPI.DTO;
using LaboAPI.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LaboAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _AuthServices;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthServices authServices, IConfiguration configuration)
        {
            _AuthServices = authServices;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Post(LoginDTO dto)
        {
            //Password
            byte[] plainPassword = Encoding.UTF8.GetBytes(dto.Password);
            string hashedPassword = Encoding.UTF8.GetString(SHA512.HashData(plainPassword));

            try
            {
                MembersRegisteredDTO member = _AuthServices.Auth(dto.Login, hashedPassword).RegisteredToAPI();
                List<Claim> Claims = new List<Claim>
                    {
                            new Claim(ClaimTypes.Email, member.Email),
                            new Claim(ClaimTypes.Role, member.RoleID.ToString()),
                            new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                    };
                string Token = _AuthServices.GenerateToken(_configuration["Jwt:Key"], Claims);
                return Ok(new TokenDTO(Token, member));
            }
            catch(AuthExceptions ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
