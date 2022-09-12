namespace Contracts.EnumData
{
    public static class DashboardModelsEnum
    {
        public enum AccessLevelEnum
        {
            View,
            Create,
            Edit,
            Delete,
            CreateOrEdit
        }

        public enum ChartTypeEnum
        {
            Donut,
            Column,
            Bar
        }
    }
}
