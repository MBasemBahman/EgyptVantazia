using static Contracts.EnumData.DBModelsEnum;

namespace TenantConfiguration.Extensions
{
    public static class ViewsIgnoreConfig
    {
        private static bool IsValid(this List<DashboardViewEnum> views)
        {
            return views != null && views.Any();
        }
    }
}
