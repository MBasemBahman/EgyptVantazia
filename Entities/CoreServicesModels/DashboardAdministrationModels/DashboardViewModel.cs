using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.DashboardAdministrationModels
{
    public class DashboardViewParameters : RequestParameters
    {
        public int Fk_Role { get; set; }
    }

    public class DashboardViewModel : LookUpEntity
    {
        [DisplayName(nameof(ViewPath))]
        public string ViewPath { get; set; }

        [DisplayName(nameof(PremissionsCount))]
        public int PremissionsCount { get; set; }

    }

    public class DashboardViewCreateOrEditModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DisplayName(nameof(ViewPath))]
        public string ViewPath { get; set; }

        public DashboardViewLangModel DashboardViewLang { get; set; }
    }

    public class DashboardViewLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}
