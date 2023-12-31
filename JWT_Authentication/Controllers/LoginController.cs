﻿using JWT_Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using JWT_Authentication.Payload.Request;
using JWT_Authentication.Payload.Response;

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
        private User AuthenticateUsers(LoginRequest user)
        {
            //check if user exist or not
            var existingUser = _dbContext.Users.Where(u => u.Username == user.Username.Trim() && u.Password == user.Password.Trim()).FirstOrDefault();
            return existingUser;
        }
        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.firstName+" "+user.lastName),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
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
        public IActionResult Login(LoginRequest user)
        {
            var user_ = AuthenticateUsers(user);
            if(user_ != null)
            {
                var _token = GenerateToken(user_);
                LoginResponse loginResponse = new LoginResponse
                {
                    FullName = user_.firstName + " " + user_.lastName,
                    Username = user_.Username,
                    Token = _token
                };
                return Ok(loginResponse);
            }
            else
            {
                return BadRequest(new {msg = "invalid username or password"});
            }
        }
    }
}
