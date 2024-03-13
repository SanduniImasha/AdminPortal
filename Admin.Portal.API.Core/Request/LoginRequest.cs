using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Request
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

}
