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
        DBContext _dbContext;
        private IConfiguration _config;
        public LoginController(IConfiguration configuration, DBContext dbContext)
        {
            _config = configuration;
            _dbContext = dbContext;
        }
        private User AuthenticateUsers(User user)
        {
            //check if user exist or not
            var existingUser = _dbContext.Users.Where(u => u.Username == user.Username && u.Password == user.Password).FirstOrDefault();
            return existingUser;
        }
        private string GenerateToken(User users)
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
        public IActionResult Login(User user)
        {
            IActionResult response = Unauthorized();
            var user_ = AuthenticateUsers(user);
            if(user_ != null)
            {
                var token = GenerateToken(user_);
                response = Ok(new 
                { 
                    token = token, 
                    user = user_.Username 
                });
                return response;
            }
            else
            {
                return BadRequest(new {msg = "invalid username or password"});
            }
        }
    }
}
