using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JWT_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("getData")]
        public IActionResult getData()
        {
            return Ok("you are authorized!!! Now you can access the data.");
        }

        [HttpGet]
        [Route("hello")]
        public IActionResult hello()
        {
            return Ok("Hello, you don't need to login to view this msg!");
        }
    }
}
