using static Contracts.EnumData.DBModelsEnum;

namespace TenantConfiguration
{
    public class TenantConfig
    {
        private List<DashboardViewEnum> _dashboardViews;

        public TenantConfig(TenantEnvironments tenant)
        {
            Tenant = tenant;
            SetSettings(tenant);
        }

        public TenantEnvironments Tenant { get; }

        public string AppSettings { get; private set; }

        public List<DashboardViewEnum> DashboardViews
        {
            get
            {
                if (_dashboardViews == null || !_dashboardViews.Any())
                {
                    SetDashboardViews(Tenant);
                }
                return _dashboardViews;
            }
        }

        public void SetSettings(TenantEnvironments tenant)
        {
            AppSettings = "appsettings." + tenant.ToString().ToLower() + ".json";
        }
        public void SetDashboardViews(TenantEnvironments tenant)
        {
            _dashboardViews = new List<DashboardViewEnum>();

            foreach (DashboardViewEnum value in Enum.GetValues(typeof(DashboardViewEnum)))
            {
                _dashboardViews.Add(value);
            }
        }
    }
}