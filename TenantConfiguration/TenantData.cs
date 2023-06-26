namespace TenantConfiguration
{
    public static class TenantData
    {
        public enum TenantEnvironments
        {
            Development
        }

        public enum TenantApis
        {
            Authentication,
            Account,
            Location,
            AppInfo,
            News,
            Sponsor,
            Standings,
            Season,
            Team,
            PlayerScore,
            PrivateLeague,
            PlayerTransfer,
            AccountTeam,
            PlayerState,
            Payment,
            Subscription,
            Notification,
            PromoCode
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
