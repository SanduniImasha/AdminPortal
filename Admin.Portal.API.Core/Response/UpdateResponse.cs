using Admin.Portal.API.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Portal.API.Core.Response
{
    public class UpdateResponse
    {
            [Required]
            public int ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            [Required]
            public string Email { get; set; }
    }
}
