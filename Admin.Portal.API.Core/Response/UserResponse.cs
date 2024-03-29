﻿using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Response
{
    public class UserResponse
    {
        [Required]
        public string ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public List<int> Tenants { get; set; }
    }
}
