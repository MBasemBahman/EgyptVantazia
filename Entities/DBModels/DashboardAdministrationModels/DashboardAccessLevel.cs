namespace Entities.DBModels.DashboardAdministrationModels
{
    public class DashboardAccessLevel : LookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }

        [DisplayName(nameof(CreateAccess))]
        public bool CreateAccess { get; set; }

        [DisplayName(nameof(EditAccess))]
        public bool EditAccess { get; set; }

        [DisplayName(nameof(ViewAccess))]
        public bool ViewAccess { get; set; }

        [DisplayName(nameof(DeleteAccess))]
        public bool DeleteAccess { get; set; }

        [DisplayName(nameof(Premissions))]
        public IList<AdministrationRolePremission> Premissions { get; set; }

        public DashboardAccessLevelLang DashboardAccessLevelLang { get; set; }
    }

    public class DashboardAccessLevelLang : LangEntity<DashboardAccessLevel>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}
