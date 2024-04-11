using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Request
{
    public class TenantDeleteRequest
    {
        [Required]
        public int ID { get; set; }
        public bool ForceDelete { get; set; } = false;
    }
}
