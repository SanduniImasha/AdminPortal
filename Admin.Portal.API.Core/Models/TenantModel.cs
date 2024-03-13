using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Models
{
    public class TenantModel
    {
        [Required]
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
