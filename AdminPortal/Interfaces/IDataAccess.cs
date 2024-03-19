using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Request;
using Admin.Portal.API.Core.Response;

namespace Admin.Portal.API.Interfaces
{
    public interface IDataAccess
    {
        public Task<bool> Validate(LoginRequest context);
        public Task<(bool, UserModel)> GetLoginUserDetails(string username);
        public Task<(bool, List<TenantModel>)> GetTenantDeatils();
        public Task<(bool, List<UserModel>)> GetUserDeatils(string? tenantID);
        public Task<(bool, TenantModel)> CreateTenant(TenantModel context);
        public Task<(bool, UserModel)> CreateUser(UserModel context);
        public Task<(bool, UserModel)> UpdateUser(UserModel context);
        public Task<bool> DeleteUser(int id);
        public Task<(bool, TenantModel)> UpdateTenant(TenantModel context);
    }
}
