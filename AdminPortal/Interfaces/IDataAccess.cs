using Admin.Portal.API.Core.Enum;
using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Request;
using Admin.Portal.API.Core.Response;

namespace Admin.Portal.API.Interfaces
{
    public interface IDataAccess
    {
        public Task<bool> Validate(LoginRequest context);
        public Task<(bool, UserModel)> GetUserDetails(string username);
        public Task<(bool, List<TenantModel>)> GetTenantDeatils(int? userID);
        public Task<(bool, TenantModel)> GetOneTenant(int tenantID);
        public Task<(bool, TenantModel)> CreateTenant(TenantModel context);
        public Task<(bool, TenantModel)> UpdateTenant(TenantModel context);
        public Task<bool> DeleteTenant(TenantDeleteRequest context);
        public Task<(bool, List<UserModel>)> GetUsers(int? tenantID);
        public Task<(bool, UserModel)> CreateUser(UserModel context);
        public Task<(bool, UserModel)> UpdateUser(UserModel context);
        public Task<(bool, UserModel)> LinkTenantToUser(UserTenantLinkRequest context);
        public Task<(bool, UserModel)> UnLinkTenantFromUser(UserTenantLinkRequest context);
        public Task<bool> DeleteUser(int id);
        public Task<(bool, List<RoleModel>)> GetRoles(int? tenantID);
        public Task<(bool, RoleModel)> GetRole(int roleID);
        public Task<(bool, RoleModel)> CreateRole(RoleModel context);
        public Task<(bool, RoleModel)> UpdateRole(RoleModel context);
        public Task<bool> DeleteRole(int? Id);
        public Task<(bool, RoleModel)> LinkClaimToRole(RoleClaimLinkRequest context);
        public Task<(bool, RoleModel)> UnLinkClaimFromRole(RoleClaimLinkRequest context);
        //public Task<(bool, TenantModel)> LinkRoleToTenant(TenantRoleRequest context);
        //public Task<(bool, TenantModel)> UnLinkRoleFromTenant(TenantRoleRequest context);
        public Task<List<RoleClaimModel>> GetUserClaims(int userID);
        public Task<List<ClaimModel>> GetClaims();
        
        public Task<UserType> GetUserType(int userID);
        public Task<(bool success, InvitationModel result)> SaveInvitation(InvitationModel context);
        public Task<UserModel> GetUserByEmail(string email);
        public Task<UserModel> GetUserById(int userId);
        public Task<(bool, InvitationModel)> GetInvitationById(int invitationId);
    }
}
