using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JWT_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        [Authorize] //use this tag when you need the authorization to access
        [HttpGet]
        [Route("getData")]
        public IActionResult getData()
        {
            var userFullName = User.FindFirst(claim => claim.Type == "name")?.Value;
            if (userFullName == null)
            {
                return Unauthorized(new {status = "failed", msg = "Sorry, you are unauthorized"});
            }
            return Ok(new
            {
                status = "success",
                msg = $"Hello {userFullName}, you are authorized!!"
            });
        }

        [HttpGet]
        [Route("hello")]
        public IActionResult hello()
        {
            return Ok("Hello, you don't need to login to view this msg!");
        }
    }
}
