namespace Entities.DBModels.DashboardAdministrationModels
{
    public class DashboardAdministrationRole : LookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }

        [DisplayName(nameof(Premissions))]
        public IList<AdministrationRolePremission> Premissions { get; set; }

        [DisplayName(nameof(Administrators))]
        public IList<DashboardAdministrator> Administrators { get; set; }

        public DashboardAdministrationRoleLang DashboardAdministrationRoleLang { get; set; }
    }

    public class DashboardAdministrationRoleLang : LangEntity<DashboardAdministrationRole>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}
