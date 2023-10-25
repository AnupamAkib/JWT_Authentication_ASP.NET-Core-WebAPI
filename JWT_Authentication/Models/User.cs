﻿using System.ComponentModel.DataAnnotations;

namespace JWT_Authentication.Models
{
    public class User
    {
        [Key]
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
