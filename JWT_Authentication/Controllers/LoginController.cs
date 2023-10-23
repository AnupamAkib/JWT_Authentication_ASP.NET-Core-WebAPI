using JWT_Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace JWT_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        public LoginController(IConfiguration configuration)
        {
            _config = configuration;
        }
        private Users AuthenticateUsers(Users user)
        {
            Users _user = null;
            if(user.Username == "admin" && user.Password == "1234")
            {
                _user = new Users { Username = "Anupam Hossain" };
            }
            return _user;
        }
        private string GenerateToken(Users users)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, users.Username),
                new Claim(JwtRegisteredClaimNames.Iss, _config["jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, _config["jwt:Audience"])
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(10), //expire after 10 minutes
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Users user)
        {
            IActionResult response = Unauthorized();
            var user_ = AuthenticateUsers(user);
            if(user_ != null)
            {
                var token = GenerateToken(user_);
                response = Ok(new { token = token, user = user_.Username });
            }
            return response;
        }
    }
}
