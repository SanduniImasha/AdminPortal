using System.ComponentModel.DataAnnotations;


namespace Admin.Portal.API.Core.Request
{
    public class UserCreateRequest
    {
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
    }
}
