using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Response
{
    public class UserResponse
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public List<int> Tenants { get; set; }
    }
}
