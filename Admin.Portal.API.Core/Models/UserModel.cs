using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Models
{
    public class UserModel
    {
        public string ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public List<string> Tenenats { get; set; }
    }
}
