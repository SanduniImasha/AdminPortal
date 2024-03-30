using Admin.Portal.API.Core.Enum;
using Admin.Portal.API.Core.Models.Base;
using Admin.Portal.API.Helpers;
using Admin.Portal.API.Interfaces;
using Admin.Portal.API.Services;

namespace Admin.Portal.API.Filters
{
    public class AuthenticateFilter
    {
        private List<string> Claims {  get; set; }
        private DataContext dbContext { get; set; }
        private string UserID { get; set; }
        private Settings config;
        public AuthenticateFilter(Settings _Settings, DataContext _dbContext) 
        {
            config = _Settings;
            dbContext = _dbContext;
        }
        public bool Authenticate(AccessLevel level, string claim, string userID, int tenantID = 0)
        {
            if (Convert.ToInt32(userID) == config.SuperUser.ID)
                return true;

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            UserType userType = db.GetUserType(Convert.ToInt32(userID)).Result;
            if (userType == UserType.AdminUser)
                return true;

            else if (level == AccessLevel.Claims)
                return db.GetUserClaims(Convert.ToInt32(userID)).Result.Where(r => r.Claim.Name == claim).ToList().Count() > 0;
            else
                return db.GetUserClaims(Convert.ToInt32(userID)).Result.Where(r => r.TenantID == tenantID && r.Claim.Name == claim).ToList().Count() > 0;
        }
    }
}
