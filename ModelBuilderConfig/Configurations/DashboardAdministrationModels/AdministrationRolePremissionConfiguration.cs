using Entities.DBModels.DashboardAdministrationModels;

namespace ModelBuilderConfig.Configurations.DashboardAdministrationModels
{
    public class AdministrationRolePremissionConfiguration : IEntityTypeConfiguration<AdministrationRolePremission>
    {
        public void Configure(EntityTypeBuilder<AdministrationRolePremission> builder)
        {
            _ = builder.HasIndex(a => new
            {
                a.Fk_DashboardAdministrationRole,
                a.Fk_DashboardView
            }).IsUnique();


            int index = 1;
            foreach (DashboardViewEnum value in Enum.GetValues(typeof(DashboardViewEnum)))
            {
                _ = builder.HasData(new AdministrationRolePremission
                {
                    Id = index++,
                    Fk_DashboardAccessLevel = (int)DashboardAccessLevelEnum.FullAccess,
                    Fk_DashboardAdministrationRole = (int)DashboardAdministrationRoleEnum.Developer,
                    Fk_DashboardView = (int)value
                });
            }

        }
    }
}
