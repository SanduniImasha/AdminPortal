using System.ComponentModel.DataAnnotations;


namespace Admin.Portal.API.Core.Request
{
    public class TenantRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
