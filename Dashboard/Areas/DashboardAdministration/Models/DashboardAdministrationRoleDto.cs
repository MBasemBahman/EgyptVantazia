using System.ComponentModel;

namespace Dashboard.Areas.DashboardAdministration.Models
{
    public class DashboardAdministrationRoleDto : DashboardAdministrationRoleModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }
    }

    public class DashboardAdministrationRoleFilter : DtParameters
    {
        public int Id { get; set; }
    }

    public class DashboardAdministrationRoleDetailsViewModel
    {
        public DashboardAdministrationRoleDetailsViewModel()
        {
            Role = new();
            Permissions = new Dictionary<string, List<string>>();
        }
        public DashboardAdministrationRoleDto Role { get; set; }
        public Dictionary<string, List<string>> Permissions { get; set; }
    }
}
