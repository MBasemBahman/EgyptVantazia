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
            AccountTeam = 30,
            Subscription = 31
        }

        public enum ScoreTypeEnum
        {
            Minutes = 34,
            GoalkeeperSaves = 35,
            Goals = 36,
            Assists = 37,
            PenaltiesSaved = 56,
            PenaltyMissed = 59,
            YellowCard = 75,
            SecondYellowCard = 79,
            RedCard = 80,
            SelfGoal = 82,
            CleanSheet = 84, // From My Side
            ReceiveGoals = 85, // From My Side
            Ranking = 86 // From My Side
        }

        public enum PlayerPositionEnum
        {
            Attacker = 1,
            Defender = 2,
            Goalkeeper = 3,
            Midfielder = 4,
            Coach = 5,
        }
    }
}
