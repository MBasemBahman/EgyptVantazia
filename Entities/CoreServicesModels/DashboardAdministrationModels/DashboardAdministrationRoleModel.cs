using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.DashboardAdministrationModels
{
    public class DashboardAdministrationRoleRequestParameters : RequestParameters
    {
        public bool GetDeveloperRole { get; set; } = true;
    }
    public class DashboardAdministrationRoleModel : LookUpEntity
    {
        [DisplayName(nameof(PremissionsCount))]
        public int PremissionsCount { get; set; }

        [DisplayName(nameof(AdministratorsCount))]
        public int AdministratorsCount { get; set; }
    }

    public class DashboardAdministrationRoleCreateOrEditViewModel
    {
        public DashboardAdministrationRoleCreateOrEditModel Role { get; set; }

        public List<RolePermissionCreateOrEditViewModel> Permissions { get; set; }
    }

    public class DashboardAdministrationRoleCreateOrEditModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        public DashboardAdministrationRoleLangModel DashboardAdministrationRoleLang { get; set; }
    }

    public class DashboardAdministrationRoleLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }

    public class RolePermissionCreateOrEditViewModel
    {
        public int Fk_AccessLevel { get; set; }
        public string AccessLevelName { get; set; }
        public List<int> Fk_Views { get; set; }
    }
}
