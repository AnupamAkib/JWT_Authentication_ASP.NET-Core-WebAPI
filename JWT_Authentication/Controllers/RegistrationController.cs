using JWT_Authentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace JWT_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        DBContext _dbContext;
        TextInfo textInfo;

        public RegistrationController(DBContext dbContext)
        {
            _dbContext = dbContext;
            textInfo = new CultureInfo("en-US", false).TextInfo; //for upper casing first char in each word
        }

        [HttpPost]
        [Route("registration")] //ekhane je routing dibo oi routing diye api request dite hobe
        public IActionResult registration(User _user) //register new user
        {
            try
            {
                var existingUser = _dbContext.Users.Where(u => u.Username == _user.Username.Trim()).FirstOrDefault(); //it almost like SELECT * FROM Users WHERE u.Username == _user.Username. The FirstOrDefault() method indicates that we will take the first value & if there is only null value, we will also take it.
                if (existingUser != null)
                {
                    return BadRequest(new { msg = "user already exist" });
                }
                else
                {
                    User newUser = new User //create a new User class
                    {
                        firstName = textInfo.ToTitleCase(_user.firstName.Trim().ToLower()),
                        lastName = textInfo.ToTitleCase(_user.lastName.Trim().ToLower()),
                        Username = _user.Username.Trim(),
                        Password = _user.Password
                    };
                    _dbContext.Users.Add(newUser);
                    int rowAffected = _dbContext.SaveChanges();
                    if (rowAffected > 0)
                    {
                        return Ok(new
                        {
                            msg = "successfully registered",
                            user = newUser
                        });
                    }
                    else
                    {
                        return BadRequest(new { msg = "something went wrong" });
                    }
                }
            }
            catch(Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }
    }
}
