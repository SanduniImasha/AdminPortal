using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Request
{
    public class UserTenantLinkRequest
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        public int TenantID { get; set; }
    }
}
