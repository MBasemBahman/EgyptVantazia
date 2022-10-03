using System.ComponentModel;

namespace Dashboard.Areas.DashboardAdministration.Models
{
    public class DashboardAccessLevelDto : DashboardAccessLevelModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }
    }

    public class DashboardAccessLevelFilter : DtParameters
    {
        public int Id { get; set; }
    }
}
