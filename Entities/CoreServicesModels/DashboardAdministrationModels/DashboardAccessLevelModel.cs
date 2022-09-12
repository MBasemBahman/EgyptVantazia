using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.DashboardAdministrationModels
{
    public class DashboardAccessLevelParameters : RequestParameters
    {
        public int Fk_DashboardAdministrationRole { get; set; }

        public int Fk_DashboardView { get; set; }
    }
    public class DashboardAccessLevelModel : LookUpEntity
    {
        [DisplayName(nameof(CreateAccess))]
        public bool CreateAccess { get; set; }

        [DisplayName(nameof(EditAccess))]
        public bool EditAccess { get; set; }

        [DisplayName(nameof(ViewAccess))]
        public bool ViewAccess { get; set; }

        [DisplayName(nameof(DeleteAccess))]
        public bool DeleteAccess { get; set; }

        [DisplayName(nameof(PremissionsCount))]
        public int PremissionsCount { get; set; }
    }

    public class DashboardAccessLevelCreateOrEditModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(CreateAccess))]
        public bool CreateAccess { get; set; }

        [DisplayName(nameof(EditAccess))]
        public bool EditAccess { get; set; }

        [DisplayName(nameof(ViewAccess))]
        public bool ViewAccess { get; set; }

        [DisplayName(nameof(DeleteAccess))]
        public bool DeleteAccess { get; set; }

        public DashboardAccessLevelLangModel DashboardAccessLevelLang { get; set; }
    }

    public class DashboardAccessLevelLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}
