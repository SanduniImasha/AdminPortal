using Admin.Portal.API.Core.Const;
using Admin.Portal.API.Core.Enum;
using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Request;
using Admin.Portal.API.Helpers;
using Admin.Portal.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Admin.Portal.API.Services
{
    public class SQLite : IDataAccess
    {
        private protected DataContext dbContext { get; set; }
        public SQLite(DataContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<bool> Validate(LoginRequest context)
        {
            if (dbContext.Users.Where(u => u.Email.ToString().ToLower() == context.Username.ToLower() && u.Password.ToString() == context.Password).Count() == 1)
                return true;
            else
                return false;
        }

        public async Task<(bool, UserModel)> GetUserDetails(string username)
        {
            UserModel user =  dbContext.Users.Where(u => u.Email.ToLower() == username.ToLower()).FirstOrDefault();

            if (user != null)
                return (true, dbContext.Users.Where(u => u.Email.ToLower() == username.ToLower()).FirstOrDefault());
            else
                return (false, null);
        }

        public async Task<(bool, List<UserModel>)> GetUsers(int? tenantID)
        {
            if(tenantID != null)
                return (true, dbContext.Users.Where(u => u.Tenants.Any(t => t.Equals(tenantID))).ToListAsync().Result);
            else
                return (true, dbContext.Users.ToListAsync().Result);
        }

      

        public async Task<(bool, UserModel)> CreateUser(UserModel context)
        {
            if (dbContext.Users.Where(u => u.Email.ToLower() == context.Email.ToLower()).ToList().Count > 1)
                throw new Exception(Messages.ERROR_USER_EXSIST);

            dbContext.Users.Add(context);
            await dbContext.SaveChangesAsync();
            return (true, dbContext.Users.Where(u => u.Email == context.Email).FirstOrDefault());
        }

        public async Task<(bool, UserModel)> UpdateUser(UserModel context)
        {
           if (dbContext.Users.Where(u => u.ID == context.ID).ToList().Count > 1)
                throw new Exception(Messages.ERROR_USER_DOES_NOT_EXSIST);

            dbContext.Users.Update(context);
            await dbContext.SaveChangesAsync();
            return (true, context);
        }

        public async Task<(bool, UserModel)> LinkTenantToUser(UserTenantLinkRequest context)
        {
            UserModel user = dbContext.Users.Where(u => u.ID == context.UserID).FirstOrDefault();
            user.Tenants.Add(context.TenantID);

            if (user.Tenants.Where(u => (int)u == context.TenantID).ToList().Count > 1)
                return (false, null);

            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return (true, user);
        }

        public async Task<(bool, UserModel)> UnLinkTenantFromUser(UserTenantLinkRequest context)
        {
            UserModel user = dbContext.Users.Where(u => u.ID == context.UserID).FirstOrDefault();

            if (user.Tenants.Where(u => (int)u == context.TenantID).ToList().Count == 0)
                return (false, null);

            user.Tenants.Remove(context.TenantID);
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return (true, user);
        }

        public async Task<bool> DeleteUser(int id)
        {
            if (dbContext.Users.Where(u => u.ID == id).ToList().Count == 0)
                return (false);

            dbContext.Users.Where(u => u.ID == id).ExecuteDelete();
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<(bool, List<TenantModel>)> GetTenantDeatils(int? userID)
        {
            if (userID != null)
            {
                List<int> lstTM = dbContext.Users.Where(u => u.ID == userID).FirstOrDefault().Tenants;
                return (true, dbContext.Tenants.Where(t => lstTM.Contains(t.ID)).ToListAsync().Result);
            }
            else
                return (true, await dbContext.Tenants.ToListAsync());
        }

        public async Task<(bool, TenantModel)> GetOneTenant(int tenantID)
        {
            if (tenantID != null)
            {
                // TenantModel t = dbContext.Tenants.Where(u => u.ID == tenantID).FirstOrDefault();
                return (true, dbContext.Tenants.Where(u => u.ID == tenantID).FirstOrDefault());
            }
            else
                return (false, null); 
        }
        public async Task<(bool, TenantModel)> CreateTenant(TenantModel context)
        {
            if (dbContext.Tenants.Where(t => t.Name.ToLower() == context.Name.ToLower()).ToList().Count > 1)
                throw new Exception(Messages.ERROR_TENANT_EXSIST);

            dbContext.Tenants.Add(context);
            await dbContext.SaveChangesAsync();
            return (true, context);
        }
        public async Task<(bool, TenantModel)> UpdateTenant(TenantModel context)
        {
          if (dbContext.Tenants.Where(u => u.ID == context.ID).ToList().Count == 0)
              return (false, null);

            dbContext.Tenants.Update(context);
            await dbContext.SaveChangesAsync();
            return (true, context);
        }

        public async Task<bool> DeleteTenant(int id)
        {
            // User Check
            if (dbContext.Users.Where(u => u.Tenants.Contains(id)).ToList().Count > 0)
                return (false);

            //Role Check
            if (dbContext.Roles.Where(u => u.TenantID == id).ToList().Count > 0)
                return (false);

            dbContext.Tenants.Where(t => t.ID == id).ExecuteDelete();
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<(bool, List<RoleModel>)> GetRoles(int? tenantID)
        {
            if (tenantID != null)     
                
                return (true, dbContext.Roles.Where(u => u.TenantID == tenantID).ToList());
            else
                return (true, await dbContext.Roles.ToListAsync());
        }

        public async Task<(bool, RoleModel)> GetRole(int roleID)
        {
            if (roleID != null)
            {
                return (true, dbContext.Roles.Where(r => r.ID == roleID).FirstOrDefault());
            }
            else
                return (false,null);
        }

        public async Task<(bool, RoleModel)> CreateRole(RoleModel context)
        {
            if (dbContext.Roles.Where(u => u.Name == context.Name && u.TenantID == context.TenantID).ToList().Count > 0)
                return (false, null);

            dbContext.Roles.Add(context);
            await dbContext.SaveChangesAsync();
            return (true, context);
        }

        public async Task<(bool, RoleModel)> UpdateRole(RoleModel context)
        {
            if (dbContext.Roles.Where(u => u.ID == context.ID).ToList().Count == 0)
                return (false, null);

            dbContext.Roles.Update(context);
            await dbContext.SaveChangesAsync();
            return (true, context);
        }

        public async Task<bool> DeleteRole(int? Id)
        {
            if (dbContext.Roles.Where(u => u.ID == Id).ToList().Count == 0)
                return (false);

            dbContext.Roles.Where(t=> t.ID == Id).ExecuteDelete();
            await dbContext.SaveChangesAsync();
            return (true);
        }

        public async Task<(bool, RoleModel)> LinkClaimToRole(RoleClaimLinkRequest context)
        {
            RoleModel role = dbContext.Roles.Where(u => u.ID == context.RoleId).FirstOrDefault();
            if (role.Claims == null) role.Claims = new();
            foreach (int claim in context.ClaimIds )
            {
                if(!role.Claims.Contains(claim))
                    role.Claims.Add(claim);
            }
            
            dbContext.Roles.Update(role);
            await dbContext.SaveChangesAsync();
            return (true, role);
        }

        public async Task<(bool, RoleModel)> UnLinkClaimFromRole(RoleClaimLinkRequest context)
        {
            RoleModel role = dbContext.Roles.Where(t => t.ID == context.RoleId).FirstOrDefault();

            foreach (int claim in context.ClaimIds) 
            {
                if (role.Claims.Contains(claim))
                    role.Claims.Remove(claim);
            }
            dbContext.Roles.Update(role);
            await dbContext.SaveChangesAsync();
            return (true, role);
        }

     /*   public async Task<(bool, TenantModel)> LinkRoleToTenant(TenantRoleRequest context)
       {
        //   TenantModel tenant = dbContext.Tenants.Where(t=>t.ID == context.TenantId).FirstOrDefault();
          // if (tenant.Roles == null) tenant.Roles = new();
           foreach(int role in context.RoleIds)
           {
               tenant.Roles.Add(role);
           }
                 dbContext.Tenants.Update(tenant);
                 await dbContext.SaveChangesAsync();
                 return (true, tenant);
        }
     */
        //public async Task<(bool, TenantModel)> UnLinkRoleFromTenant(TenantRoleRequest context)
        //{
        //    TenantModel tenant = dbContext.Tenants.Where(t => t.ID == context.TenantId).FirstOrDefault();
        //    foreach (int role in context.RoleIds)
        //    {
        //        tenant.Roles.Remove(role);
        //    }
        //    dbContext.Tenants.Remove(tenant);
        //    await dbContext.SaveChangesAsync();
        //    return (true, tenant);
        //}

        public async Task<List<RoleClaimModel>> GetUserClaims(int userID)
        {
            List<int> tenants = dbContext.Users.Where(u => u.ID == userID).FirstOrDefault().Tenants;
            List<RoleModel> roles = dbContext.Roles.Where(r => tenants.Contains(r.TenantID)).ToList();

            List<RoleClaimModel> claims = [];
            foreach (RoleModel role in roles)
                foreach (int claim in role.Claims)
                    claims.Add(new() { ID = role.ID, TenantID = role.TenantID, Claim = dbContext.Claims.Where(c => c.Id == claim).FirstOrDefault() });

            return claims;
        }
        public async Task<List<ClaimModel>> GetClaims()
        {
            return (dbContext.Claims.ToListAsync().Result);
        }
        public async Task<UserType> GetUserType(int userID)
        {
            UserModel user = dbContext.Users.Where(u => u.ID == userID).FirstOrDefault();
            return user.Type;
        }
        public async Task<UserModel> GetUserByEmail(string email)
        {

            return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);


        }
        public async Task<UserModel> GetUserById(int userId)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.ID == userId);
        }
        public async Task<(bool, InvitationModel)> SaveInvitation(InvitationModel context)
        {
            try
            {
                dbContext.Invitations.Add(context);
                await dbContext.SaveChangesAsync();


                UserModel receiverUser = await dbContext.Users.FirstOrDefaultAsync(u => u.ID == context.ReceiverID);
                if (receiverUser != null)
                {
                    receiverUser.Invitations.Add(context.ID);
                    await dbContext.SaveChangesAsync();
                }

                return (true, context);
            }
            catch (Exception ex)
            {

                return (false, null);
            }
        }
        public async Task<(bool, InvitationModel)> GetInvitationById(int invitationId)
        {
            try
            {
                var invitation = await dbContext.Invitations.FirstOrDefaultAsync(i => i.ID == invitationId);
                return (invitation != null, invitation);
            }
            catch (Exception ex)
            {

                return (false, null);
            }
        }




    }
}
