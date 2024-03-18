using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Request;
using Admin.Portal.API.Filters;
using Admin.Portal.API.Helpers;
using Admin.Portal.API.Interfaces;
using Microsoft.EntityFrameworkCore;
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
            throw new NotImplementedException();
        }

        public async Task<(bool, UserModel)> GetLoginUserDetails(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool, List<TenantModel>)> GetTenantDeatils()
        {
            throw new NotImplementedException();
        }

        public async Task<(bool, List<UserModel>)> GetUserDeatils(string? tenantID)
        {
            if(!String.IsNullOrEmpty(tenantID))
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
    }
}
