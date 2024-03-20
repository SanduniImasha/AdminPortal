using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Models
{
    public class TenantModel
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
