using Admin.Portal.API.Core.Enum;

namespace Admin.Portal.API.Core.Models.Base
{
    public class Settings
    {
        public string RootPath { get; set; }
        public UserModel SuperUser { get; set; }
        public DB DataAccess {  get; set; }
       
    }

    public class DB
    {
        public DBProvider Provider { get; set; }
        public string File { get; set; }
    }

}
