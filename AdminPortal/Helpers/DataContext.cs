using Admin.Portal.API.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Admin.Portal.API.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }


        public DbSet<UserModel> Users => Set<UserModel>();
        public DbSet<TenantModel> Tenants => Set<TenantModel>();
        public DbSet<RoleModel> Roles => Set<RoleModel>();
        public DbSet<ClaimModel> Claims => Set<ClaimModel>();
        public DbSet<InvitationModel> Invitations => Set<InvitationModel>();
    }
}
