using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwilioRegistration.BusinessLogic.Data
{
    public class Context : DbContext
    {
        public Context() : base("name=DbConnectionString") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Account>()
                .HasMany(x => x.Roles)
                .WithMany(x => x.Accounts)
            .Map(x =>
            {
                x.ToTable("AccountsRoles");
                x.MapLeftKey("AccountId");
                x.MapRightKey("RoleId");
            });

            modelBuilder.Entity<Role>()
                .HasMany(x => x.Permissions)
                .WithMany(x => x.Roles)
            .Map(x =>
            {
                x.ToTable("RolesPermissions");
                x.MapLeftKey("RoleId");
                x.MapRightKey("PermissionId");
            });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Server> Servers { get; set; }

        public DbSet<Device> Devices { get; set; }

        public override int SaveChanges()
        {
            DateTime saveTime = DateTime.UtcNow;
            foreach (var entry in this.ChangeTracker.Entries().Where(e => e.State == System.Data.Entity.EntityState.Added))
            {
                if (entry.Property("CreatedAt").CurrentValue == null)
                    entry.Property("CreatedAt").CurrentValue = saveTime;
                if (entry.Property("UpdatedAt").CurrentValue == null)
                    entry.Property("UpdatedAt").CurrentValue = saveTime;
            }
            return base.SaveChanges();
        }
    }
}
