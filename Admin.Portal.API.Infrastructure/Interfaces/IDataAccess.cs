using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Request;

namespace Admin.Portal.API.Infrastructure.Interfaces
{
    public interface IDataAccess
    {
        public Task<bool> Validate(LoginRequest context);
        public Task<(bool, ProfileModel)> GetLoginUserDetails(string username);
        public Task<(bool, List<TenantModel>)> GetTenantDeatils();
        public Task<(bool, List<UserModel>)> GetUserDeatils(string? tenantID);
        public Task<(bool, TenantModel)> CreateTenant(TenantModel context);
        public Task<(bool, UserModel)> CreateUser(UserModel context);
    }
}
