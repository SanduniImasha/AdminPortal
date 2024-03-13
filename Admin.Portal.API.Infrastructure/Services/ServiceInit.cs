using Admin.Portal.API.Core.Enum;
using Admin.Portal.API.Core.Models.Base;

namespace Admin.Portal.API.Infrastructure.Services
{
    public class ServiceInit
    {
        public static dynamic GetDataInstance(Settings providerInfo)
        {
            DB info = new()
            {
                File = providerInfo.RootPath + providerInfo.DataAccess.File,
                Provider = providerInfo.DataAccess.Provider
            };

            switch (info.Provider)
            {
                case DBProvider.TextFile:
                    return new JsonTextDB(info);
                case DBProvider.SQLLite:
                    return new SQLLite(info);
                default:
                    return new SQLLite(info);
            }
        }
    }
}
