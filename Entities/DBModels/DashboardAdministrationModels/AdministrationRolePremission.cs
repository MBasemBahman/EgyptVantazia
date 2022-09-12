namespace Entities.DBModels.DashboardAdministrationModels
{
    public class AdministrationRolePremission : BaseEntity
    {
        [ForeignKey(nameof(DashboardAccessLevel))]
        [DisplayName(nameof(DashboardAccessLevel))]
        public int Fk_DashboardAccessLevel { get; set; }

        [DisplayName(nameof(DashboardAccessLevel))]
        public DashboardAccessLevel DashboardAccessLevel { get; set; }

        [ForeignKey(nameof(DashboardAdministrationRole))]
        [DisplayName(nameof(DashboardAdministrationRole))]
        public int Fk_DashboardAdministrationRole { get; set; }

        [DisplayName(nameof(DashboardAdministrationRole))]
        public DashboardAdministrationRole DashboardAdministrationRole { get; set; }

        [ForeignKey(nameof(DashboardView))]
        [DisplayName(nameof(DashboardView))]
        public int Fk_DashboardView { get; set; }

        [DisplayName(nameof(DashboardView))]
        public DashboardView DashboardView { get; set; }
    }
}
