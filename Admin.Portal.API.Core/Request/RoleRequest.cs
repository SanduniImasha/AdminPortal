using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Portal.API.Core.Request
{
    public class RoleRequest
    {
        [Required]
        public string Name { get; set; }
        public List<int> Claims { get; set; }
    }
}
