using Admin.Portal.API.Core.Const;
using Admin.Portal.API.Core.Enum;
using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Models.Base;
using Admin.Portal.API.Core.Request;
using Admin.Portal.API.Core.Response;
using Admin.Portal.API.Filters;
using Admin.Portal.API.Helpers;
using Admin.Portal.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;

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

        public async Task<(bool, List<UserModel>)> GetUsers(int? tenantID)
        {
            if(tenantID != null)
                return (true, dbContext.Users.Where(u => u.Tenants.Any(t => t.Equals(tenantID))).ToListAsync().Result);
            else
                return (true, dbContext.Users.ToListAsync().Result);
        }

        public async Task<(bool, TenantModel)> CreateTenant(TenantModel context)
        {
            dbContext.Tenants.Add(context);
            await dbContext.SaveChangesAsync(); 
            return (true, context);
        }

        public async Task<(bool, UserModel)> CreateUser(UserModel context)
        {
            dbContext.Users.Add(context);
            await dbContext.SaveChangesAsync();
            return (true, dbContext.Users.Where(u => u.Email == context.Email).FirstOrDefault());
        }

        public async Task<(bool, UserModel)> UpdateUser(UserModel context)
        {
            dbContext.Users.Update(context);
            await dbContext.SaveChangesAsync();
            return (true, context);
        }

        public async Task<(bool, UserModel)> LinkTenantToUser(UserTenantLinkRequest context)
        {
            UserModel user = dbContext.Users.Where(u => u.ID == context.UserID).FirstOrDefault();
            user.Tenants.Add(context.TenantID);

            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return (true, user);
        }

        public async Task<(bool, UserModel)> UnLinkTenantFromUser(UserTenantLinkRequest context)
        {
            UserModel user = dbContext.Users.Where(u => u.ID == context.UserID).FirstOrDefault();
            user.Tenants.Remove(context.TenantID);
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return (true, user);
        }

        public async Task<bool> DeleteUser(int id)
        {
            dbContext.Users.Where(u => u.ID == id).ExecuteDelete();
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<(bool, TenantModel)> UpdateTenant(TenantModel context)
        {
            dbContext.Tenants.Update(context);
            await dbContext.SaveChangesAsync();
            return (true, context);
        }

        public async Task<bool> DeleteTenant(int id)
        {
            dbContext.Tenants.Where(t => t.ID == id).ExecuteDelete();
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<(bool, List<RoleModel>)> GetRoles(int? tenantID)
        {
            if (tenantID != null)
            {
                List<int> roleList = dbContext.Tenants.Where(u => u.ID == tenantID).FirstOrDefault().Roles;
                return (true, dbContext.Roles.Where(t => roleList.Contains(t.Id)).ToListAsync().Result);
            }
            else
                return (true, await dbContext.Roles.ToListAsync());
        }

        public async Task<(bool, RoleModel)> CreateRole(RoleModel context)
        {
            dbContext.Roles.Add(context);
            await dbContext.SaveChangesAsync();
            return (true, context);
        }

        public async Task<(bool, RoleModel)> UpdateRole(RoleModel context)
        {
            dbContext.Roles.Update(context);
            await dbContext.SaveChangesAsync();
            return (true, context);
        }

        public async Task<bool> DeleteRole(int? Id)
        {
            dbContext.Roles.Where(t=> t.Id == Id).ExecuteDelete();
            await dbContext.SaveChangesAsync();
            return (true);
        }

        public async Task<(bool, RoleModel)> LinkClaimToRole(RoleClaimLinkRequest context)
        {
            RoleModel role = dbContext.Roles.Where(u => u.Id == context.RoleId).FirstOrDefault();
            if (role.Claims == null) role.Claims = new();
            foreach (int claim in context.ClaimIds )
            {
                role.Claims.Add(claim);
            }
            
            dbContext.Roles.Update(role);
            await dbContext.SaveChangesAsync();
            return (true, role);
        }

        public async Task<(bool, RoleModel)> UnLinkClaimFromRole(RoleClaimLinkRequest context)
        {
            RoleModel role = dbContext.Roles.Where(t => t.Id == context.RoleId).FirstOrDefault();
            foreach (int claim in context.ClaimIds) 
            {
                role.Claims.Remove(claim);
            }
            dbContext.Roles.Update(role);
            await dbContext.SaveChangesAsync();
            return (true, role);
        }

        public async Task<(bool, TenantModel)> LinkRoleToTenant(TenantRoleRequest context)
        {
            TenantModel tenant = dbContext.Tenants.Where(t=>t.ID == context.TenantId).FirstOrDefault();
            if (tenant.Roles == null) tenant.Roles = new();
            foreach(int role in context.RoleIds)
            {
                tenant.Roles.Add(role);
            }
            dbContext.Tenants.Update(tenant);
            await dbContext.SaveChangesAsync();
            return (true, tenant);
        }

        public async Task<(bool, TenantModel)> UnLinkRoleFromTenant(TenantRoleRequest context)
        {
            TenantModel tenant = dbContext.Tenants.Where(t => t.ID == context.TenantId).FirstOrDefault();
            foreach (int role in context.RoleIds)
            {
                tenant.Roles.Remove(role);
            }
            dbContext.Tenants.Remove(tenant);
            await dbContext.SaveChangesAsync();
            return (true, tenant);
        }

        public async Task<List<string>> GetUserClaims(int userID)
        {
            List<int> tenants = dbContext.Users.Where(u => u.ID == userID).FirstOrDefault().Tenants;
            List<List<int>> roles = dbContext.Tenants.Where(t => tenants.Contains(t.ID)).Select(t => t.Roles).ToList();
            List<int> uniqueRoles = [];
            foreach (List<int> x in roles)
                foreach (int item in x)
                    if (!uniqueRoles.Contains(item))
                        uniqueRoles.Add(item);


            List<List<int>> roleClaims = dbContext.Roles.Where(r => uniqueRoles.Contains(r.Id)).Select(c => c.Claims).ToList();
            List<int> uniqueClaims = [];
            foreach (List<int> x in roleClaims)
                foreach (int item in x)
                    if (!uniqueClaims.Contains(item))
                        uniqueClaims.Add(item);
            
            return dbContext.Claims.Where(c => uniqueClaims.Contains(c.Id)).Select(c => c.Name).ToList();
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
    }
}
