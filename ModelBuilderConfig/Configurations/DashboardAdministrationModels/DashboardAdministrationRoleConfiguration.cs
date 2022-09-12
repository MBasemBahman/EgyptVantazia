using Entities.DBModels.DashboardAdministrationModels;

namespace ModelBuilderConfig.Configurations.DashboardAdministrationModels
{
    public class DashboardAdministrationRoleConfiguration : IEntityTypeConfiguration<DashboardAdministrationRole>
    {
        public void Configure(EntityTypeBuilder<DashboardAdministrationRole> builder)
        {
            _ = builder.HasData(new DashboardAdministrationRole
            {
                Id = (int)DashboardAdministrationRoleEnum.Developer,
                Name = DashboardAdministrationRoleEnum.Developer.ToString()
            });
        }
    }

    public class DashboardAdministrationRoleLangConfiguration : IEntityTypeConfiguration<DashboardAdministrationRoleLang>
    {
        public void Configure(EntityTypeBuilder<DashboardAdministrationRoleLang> builder)
        {
            _ = builder.HasData(new DashboardAdministrationRoleLang
            {
                Id = (int)DashboardAdministrationRoleEnum.Developer,
                Name = DashboardAdministrationRoleEnum.Developer.ToString(),
                Fk_Source = (int)DashboardAdministrationRoleEnum.Developer
            });
        }
    }
}
