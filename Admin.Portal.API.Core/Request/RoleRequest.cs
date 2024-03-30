using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Request
{
    public class RoleRequest
    {
        [Required]
        public string Name { get; set; }
        public List<int> Claims { get; set; }
        [Required]
        public int TenantID { get; set; }
    }
}
