using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Models
{
    public class ProfileModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
