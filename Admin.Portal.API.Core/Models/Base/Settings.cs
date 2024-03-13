using Admin.Portal.API.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Portal.API.Core.Models.Base
{
    public class Settings
    {
        public string RootPath { get; set; }
        public DB DataAccess {  get; set; }
       
    }

    public class DB
    {
        public DBProvider Provider { get; set; }
        public string File { get; set; }
    }

}
