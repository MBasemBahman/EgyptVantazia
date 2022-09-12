using BaseDB;
using Microsoft.EntityFrameworkCore;
using ModelBuilderConfig.Configurations.DashboardAdministrationModels;
using TenantConfiguration;

namespace Live
{
    public class LiveDBContext : BaseContext
    {
        public LiveDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            TenantConfig config = new(TenantEnvironments.GAH);

            #region DashboardAdministrationModels

            _ = modelBuilder.ApplyConfiguration(new DashboardViewConfiguration(config.DashboardViews));
            _ = modelBuilder.ApplyConfiguration(new DashboardViewLangConfiguration(config.DashboardViews));

            #endregion
        }
    }
}
