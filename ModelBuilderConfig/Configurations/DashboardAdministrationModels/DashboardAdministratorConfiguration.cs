using Entities.DBModels.DashboardAdministrationModels;

namespace ModelBuilderConfig.Configurations.DashboardAdministrationModels
{
    public class DashboardAdministratorConfiguration : IEntityTypeConfiguration<DashboardAdministrator>
    {
        public void Configure(EntityTypeBuilder<DashboardAdministrator> builder)
        {
            _ = builder.HasData(new DashboardAdministrator
            {
                Id = 1,
                Fk_User = 1,
                JobTitle = DashboardAdministrationRoleEnum.Developer.ToString(),
                Fk_DashboardAdministrationRole = (int)DashboardAdministrationRoleEnum.Developer
            });
        }
    }
}
