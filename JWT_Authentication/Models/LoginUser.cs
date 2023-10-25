using System.ComponentModel.DataAnnotations;

namespace JWT_Authentication.Models
{
    public class LoginUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
