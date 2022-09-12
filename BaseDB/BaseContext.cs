using Entities.DBModels.DashboardAdministrationModels;
using Entities.DBModels.SharedModels;
using Entities.DBModels.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using ModelBuilderConfig.Configurations.DashboardAdministrationModels;
using ModelBuilderConfig.Configurations.UserModels;

namespace BaseDB
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions options) : base(options)
        {
        }

        #region UserModels

        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Verification> Verifications { get; set; }

        #endregion

        #region DashboardAdministrationModels

        public DbSet<AdministrationRolePremission> AdministrationRolePremissions { get; set; }
        public DbSet<DashboardAccessLevel> DashboardAccessLevels { get; set; }
        public DbSet<DashboardAdministrationRole> DashboardAdministrationRoles { get; set; }
        public DbSet<DashboardAdministrator> DashboardAdministrators { get; set; }
        public DbSet<DashboardView> DashboardViews { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes()
               .Where(t => t.ClrType.IsSubclassOf(typeof(BaseEntity))))
            {
                _ = modelBuilder.Entity(
                    entityType.Name,
                    x =>
                    {
                        _ = x.Property("CreatedAt")
                            .HasDefaultValueSql("getutcdate()");
                    });
            }

            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes()
               .Where(t => t.ClrType.IsSubclassOf(typeof(AuditEntity))))
            {
                _ = modelBuilder.Entity(
                    entityType.Name,
                    x =>
                    {
                        _ = x.Property("LastModifiedAt")
                            .HasDefaultValueSql("getutcdate()");
                    });
            }

            #region UserModels
            _ = modelBuilder.ApplyConfiguration(new UserConfiguration());
            #endregion

            #region DashboardAdministrationModels

            _ = modelBuilder.ApplyConfiguration(new DashboardAccessLevelConfiguration());
            _ = modelBuilder.ApplyConfiguration(new DashboardAccessLevelLangConfiguration());
            _ = modelBuilder.ApplyConfiguration(new DashboardAdministrationRoleConfiguration());
            _ = modelBuilder.ApplyConfiguration(new DashboardAdministrationRoleLangConfiguration());
            _ = modelBuilder.ApplyConfiguration(new AdministrationRolePremissionConfiguration());
            _ = modelBuilder.ApplyConfiguration(new DashboardAdministratorConfiguration());

            #endregion

        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(
           bool acceptAllChangesOnSuccess,
           CancellationToken cancellationToken = default
        )
        {
            OnBeforeSaving();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess,
                          cancellationToken);
        }

        private void OnBeforeSaving()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries();
            DateTime utcNow = DateTime.UtcNow;

            foreach (EntityEntry entry in entries)
            {
                if (entry.Entity is BaseEntity basetrackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;
                            break;

                        case EntityState.Added:
                            basetrackable.CreatedAt = utcNow;
                            break;
                    }
                }
                else if (entry.Entity is AuditEntity audittrackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            audittrackable.LastModifiedAt = utcNow;
                            entry.Property(nameof(AuditEntity.CreatedAt)).IsModified = false;
                            break;

                        case EntityState.Added:
                            audittrackable.CreatedAt = utcNow;
                            audittrackable.LastModifiedAt = utcNow;
                            break;
                    }
                }

            }
        }
    }
}