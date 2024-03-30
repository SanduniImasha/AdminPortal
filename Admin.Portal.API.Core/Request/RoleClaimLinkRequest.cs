using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Request
{
    public class RoleClaimLinkRequest
    {
        [Required]
        public int RoleId { get; set; }
        [Required]
        public List<int> ClaimIds { get; set; }
        
       
    }
}
