using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Models
{
    public class RoleModel
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int TenantID { get; set; }
        public List<int> Claims { get; set; } = new();
    }

    public class RoleClaimModel
    {
        [Required]
        public int ID { get; set; }
        public int TenantID { get; set; }
        public ClaimModel Claim { get; set; } = new();
    }
}
