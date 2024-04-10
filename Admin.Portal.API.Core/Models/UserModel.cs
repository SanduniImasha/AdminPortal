using Admin.Portal.API.Core.Enum;
using System.ComponentModel.DataAnnotations;

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
        public List<int> Tenants { get; set; }
        public List<int> Invitations { get; set; } = new List<int>();
        public UserType Type { get; set; } = UserType.NormalUser;
    }
}
