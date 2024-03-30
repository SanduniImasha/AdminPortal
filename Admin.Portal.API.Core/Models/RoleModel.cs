using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Models
{
    public class RoleModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<int> Claims { get; set; } = new();
    }
}
