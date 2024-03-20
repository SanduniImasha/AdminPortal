using Admin.Portal.API.Core.Const;
using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Models.Base;
using Admin.Portal.API.Core.Request;
using Admin.Portal.API.Core.Response;
using Admin.Portal.API.Filters;
using Admin.Portal.API.Helpers;
using Admin.Portal.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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

            if (user != null && user.Email.Length > 0)
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

                // UserModel usr = dbContext.Users.Where(u => u.ID == userID).FirstOrDefault();

                // usr.Tenants
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
            return (true, dbContext.Users.Where(u => u.ID == context.ID).FirstOrDefault());
        }

        public async Task<(bool, UserModel)> LinkTenantToUser(UserTenantLinkRequest context)
        {
            UserModel user = dbContext.Users.Where(u => u.ID == context.UserID).FirstOrDefault();
            user.Tenants.Add(context.TenantID);

            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return (true, dbContext.Users.Where(u => u.ID == context.UserID).FirstOrDefault());
        }

        public async Task<(bool, UserModel)> UnLinkTenantFromUser(UserTenantLinkRequest context)
        {
            UserModel user = dbContext.Users.Where(u => u.ID == context.UserID).FirstOrDefault();
            user.Tenants.Remove(context.TenantID);
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return (true, dbContext.Users.Where(u => u.ID == context.UserID).FirstOrDefault());
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
            return (true, dbContext.Tenants.Where(u => u.ID == context.ID).FirstOrDefault());
        }

        public async Task<bool> DeleteTenant(int id)
        {
            dbContext.Tenants.Where(t => t.ID == id).ExecuteDelete();
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
