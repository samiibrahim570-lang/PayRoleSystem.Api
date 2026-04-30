using PayRoleSystem.Core.Aspects.Logging;
using PayRoleSystem.DTOs.Response;
using PayRoleSystem.Models;
using PayRoleSystem.Models.PayRoleSystem.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace PayRoleSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly AuditLoggingInterceptor _auditLoggingInterceptor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditLoggingInterceptor auditLoggingInterceptor)
            : base(options)
        {
            _auditLoggingInterceptor = auditLoggingInterceptor;
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditLoggingInterceptor);
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<ApplicationUser> applicationUser { get; set; }
        public DbSet<Models.Page> page { get; set; }
        public DbSet<Role> role { get; set; }
        public DbSet<RolePagePermission> rolePagePermission { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
