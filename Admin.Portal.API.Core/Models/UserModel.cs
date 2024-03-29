﻿using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Models
{
    public class UserModel
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public List<string> Roles { get; set; } = new(); // Admin or User
        public List<int> Tenants { get; set; }
    }
}
