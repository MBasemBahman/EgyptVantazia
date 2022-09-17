namespace TenantConfiguration
{
    public static class TenantData
    {
        public enum TenantEnvironments
        {
            Development,
            Live,
        }

        public enum TenantApis
        {
            Authentication,
            Location,
            AppInfo,
            News,
            Sponsor,
            Standings,
            Season,
            Team,
            PlayerScore
        }

        public enum TenantViews
        {
            #region Base
            Home,

            // User
            User,
            RefreshToken,
            UserDevice,
            Verification,

            #endregion
        }
    }
}
