namespace TenantConfiguration
{
    public enum TenantEnvironments
    {
        Development,
        GAH,
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
