using Admin.Portal.API.Core.Enum;
using Admin.Portal.API.Core.Models.Base;
using Admin.Portal.API.Helpers;

namespace Admin.Portal.API.Services
{
    public class ServiceInit
    {
        public static dynamic GetDataInstance(Settings providerInfo, DataContext? dbContext = null)
        {
            if (providerInfo.DataAccess.Provider == DBProvider.TextFile)
            {
                DB info = new()
                {
                    File = providerInfo.RootPath + providerInfo.DataAccess.File,
                    Provider = providerInfo.DataAccess.Provider
                };

                return new JsonTextDB(info);
            }
            else if (providerInfo.DataAccess.Provider == DBProvider.SQLite)
            {
                return new SQLite(dbContext);
            }
            else
                return new SQLite(dbContext);
        }
    }
}
