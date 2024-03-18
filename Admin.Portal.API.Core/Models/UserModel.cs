using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public List<string> Tenants { get; set; }
    }
}
