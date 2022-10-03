using System.ComponentModel;

namespace Dashboard.Areas.DashboardAdministration.Models
{
    public class DashboardViewDto : DashboardViewModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }
    }

    public class DashboardViewFilter : DtParameters
    {
        public int Id { get; set; }
    }
}
