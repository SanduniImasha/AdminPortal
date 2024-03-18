using Admin.Portal.API.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.Portal.API.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }


        public DbSet<UserModel> Users => Set<UserModel>();
        public DbSet<TenantModel> Tenants => Set<TenantModel>();
    }
}
