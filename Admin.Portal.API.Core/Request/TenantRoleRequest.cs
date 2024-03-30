using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Request
{
    public class TenantRoleRequest
    {
        [Required]
        public int TenantId { get; set; }
        [Required]
        public List<int> RoleIds { get; set; }
    }
}
