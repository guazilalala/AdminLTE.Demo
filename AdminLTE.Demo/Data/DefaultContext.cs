using AdminLTE.Demo.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.Demo.Data
{
    public class DefaultContext:DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options):base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<RoleMenu>()
                .HasKey(rm => new { rm.RoleId, rm.MenuId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
