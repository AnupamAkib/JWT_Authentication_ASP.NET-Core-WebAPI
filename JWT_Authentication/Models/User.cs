using System.ComponentModel.DataAnnotations;

namespace JWT_Authentication.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
