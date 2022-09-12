using Entities.DBModels.DashboardAdministrationModels;

namespace ModelBuilderConfig.Configurations.DashboardAdministrationModels
{
    public class DashboardAccessLevelConfiguration : IEntityTypeConfiguration<DashboardAccessLevel>
    {
        public void Configure(EntityTypeBuilder<DashboardAccessLevel> builder)
        {
            _ = builder.HasData(new DashboardAccessLevel
            {
                Id = (int)DashboardAccessLevelEnum.FullAccess,
                Name = DashboardAccessLevelEnum.FullAccess.ToString(),
                ViewAccess = true,
                CreateAccess = true,
                EditAccess = true,
                DeleteAccess = true,
            });

            _ = builder.HasData(new DashboardAccessLevel
            {
                Id = (int)DashboardAccessLevelEnum.DataControl,
                Name = DashboardAccessLevelEnum.DataControl.ToString(),
                ViewAccess = true,
                CreateAccess = true,
                EditAccess = true,
                DeleteAccess = false,
            });

            _ = builder.HasData(new DashboardAccessLevel
            {
                Id = (int)DashboardAccessLevelEnum.Viewer,
                Name = DashboardAccessLevelEnum.Viewer.ToString(),
                ViewAccess = true,
                CreateAccess = false,
                EditAccess = false,
                DeleteAccess = false,
            });
        }
    }

    public class DashboardAccessLevelLangConfiguration : IEntityTypeConfiguration<DashboardAccessLevelLang>
    {
        public void Configure(EntityTypeBuilder<DashboardAccessLevelLang> builder)
        {
            _ = builder.HasData(new DashboardAccessLevelLang
            {
                Id = (int)DashboardAccessLevelEnum.FullAccess,
                Name = DashboardAccessLevelEnum.FullAccess.ToString(),
                Fk_Source = (int)DashboardAccessLevelEnum.FullAccess
            });

            _ = builder.HasData(new DashboardAccessLevelLang
            {
                Id = (int)DashboardAccessLevelEnum.DataControl,
                Name = DashboardAccessLevelEnum.DataControl.ToString(),
                Fk_Source = (int)DashboardAccessLevelEnum.DataControl
            });

            _ = builder.HasData(new DashboardAccessLevelLang
            {
                Id = (int)DashboardAccessLevelEnum.Viewer,
                Name = DashboardAccessLevelEnum.Viewer.ToString(),
                Fk_Source = (int)DashboardAccessLevelEnum.Viewer
            });
        }
    }
}
