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
            Minutes = 34, // الدقائق
            GoalkeeperSaves = 35, // تصديات حارس المرمى
            Goals = 36, // الأهداف
            Assists = 37, // صناعة
            PenaltiesSaved = 56, // انقاذ ركلات جزاء
            PenaltyMissed = 59, // ضربة جزاء ضائعة
            YellowCard = 75, // بطاقة صفراء
            SecondYellowCard = 79,
            RedCard = 80, // بطاقة حمراء
            SelfGoal = 82, // هدف - هدف ذاتي
            CleanSheet = 84, // From My Side - بدون اهداف
            ReceiveGoals = 85, // From My Side - تلقي اهداف
            Ranking = 86, // From My Side - الترتيب
            Goal = 118, // هدف - هدف
            Substitution = 119, // Substitution
            PenaltyKick = 120 // هدف - ركلة جزاء
        }

        public enum PlayerPositionEnum
        {
            Attacker = 1,
            Defender = 2,
            Goalkeeper = 3,
            Midfielder = 4,
            Coach = 5,
        }

        public enum ScoreStateEnum
        {
            Total = 1,
            CleanSheet = 2,
            Goals = 3,
            Assists = 4,
            GoalkeeperSaves = 5,
            PenaltiesSaved = 6,
            YellowCard = 7,
            RedCard = 8,
            BuyingPrice = 9,
            SellingPrice = 10,
            BuyingCount = 11,
            SellingCount = 12,
            PlayerSelection = 13,
            PlayerCaptain = 14
        }
    }
}
