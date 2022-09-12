using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.DashboardAdministrationModels
{
    public class AdministrationRolePremissionParameters : RequestParameters
    {
        public int Fk_DashboardAdministrationRole { get; set; }
        public int Fk_DashboardAccessLevel { get; set; }
        public int Fk_DashboardView { get; set; }
    }

    public class AdministrationRolePremissionModel : BaseEntity
    {
        [DisplayName(nameof(DashboardAccessLevel))]
        public int Fk_DashboardAccessLevel { get; set; }

        [DisplayName(nameof(DashboardAccessLevel))]
        public DashboardAccessLevelModel DashboardAccessLevel { get; set; }

        [DisplayName(nameof(DashboardAdministrationRole))]
        public int Fk_DashboardAdministrationRole { get; set; }

        [DisplayName(nameof(DashboardAdministrationRole))]
        public DashboardAdministrationRoleModel DashboardAdministrationRole { get; set; }

        [DisplayName(nameof(DashboardView))]
        public int Fk_DashboardView { get; set; }

        [DisplayName(nameof(DashboardView))]
        public DashboardViewModel DashboardView { get; set; }
    }
}
