namespace Contracts.EnumData
{
    public static class DBModelsEnum
    {
        public enum DashboardAccessLevelEnum
        {
            FullAccess = 1,
            DataControl = 2,
            Viewer = 3
        }

        public enum DashboardAdministrationRoleEnum
        {
            Developer = 1,
        }

        public enum DashboardViewEnum
        {
            Home = 1,
            User = 2,
            DashboardAdministrator = 3,
            DashboardAccessLevel = 4,
            DashboardAdministrationRole = 5,
            DashboardView = 6,
            RefreshToken = 7,
            UserDevice = 8,
            Verification = 9,
            Account = 10,
            Country = 11,
            DBLogs = 12,
            AppAbout = 13,
            Team = 14,
            News = 15,
            NewsAttachment = 16,
            Sponsor = 17,
            PlayerPosition = 18,
            Player = 19,
            PrivateLeague = 20,
            PrivateLeagueMember = 21,
            ScoreType = 22,
            TeamPlayerType = 23,
            PlayerGameWeak = 24,
            PlayerGameWeakScore = 25,
            Season = 26,
            GameWeak = 27,
            TeamGameWeak = 28,
            Standings = 29,
            AccountTeam = 30
        }
    }
}
