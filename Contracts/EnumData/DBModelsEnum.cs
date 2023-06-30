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
            Onwer = 8
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
            Subscription = 31,
            ScoreState = 32,
            PlayerGameWeakScoreState = 33,
            PlayerSeasonScoreState = 34,
            AccountSubscription = 35,
            Payment = 36,
            AccountTeamPlayer = 38,
            AccountTeamGameWeak = 39,
            Notification = 40,
            Mark = 41,
            PlayerMark = 42,
            PromoCode = 43,
            StatisticScore = 44,
            StatisticCategory = 45,
            MatchStatisticScore = 46
        }

        public enum ScoreTypeEnum
        {
            Minutes = 34, // الدقائق
            GoalkeeperSaves = 35, // تصديات حارس المرمى
            Goals = 36, // الأهداف
            Assists = 37, // صناعة
            PenaltiesSaved = 56, // انقاذ ركلات جزاء
            PenaltyMissed = 59, // ضربة جزاء ضائعة

            YellowCard_Event = 75, // بطاقة صفراء
            SecondYellowCard_Event = 79,
            RedCard_Event = 80, // بطاقة حمراء
            SelfGoal_Event = 82, // هدف - هدف ذاتي

            CleanSheet = 84, // From My Side - بدون اهداف
            ReceiveGoals = 85, // From My Side - تلقي اهداف
            Ranking = 86, // From My Side - الترتيب

            Goal_Event = 118, // هدف - هدف
            Substitution_Event = 119, // Substitution
            PenaltyKick_Event = 120 // هدف - ركلة جزاء
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
            // Points
            Total = 1,

            // Points and Value
            CleanSheet = 2,
            Goals = 3,
            Assists = 4,
            GoalkeeperSaves = 5,
            PenaltiesSaved = 6,
            YellowCard = 7,
            RedCard = 8,

            // Value
            BuyingPrice = 9,
            SellingPrice = 10,
            BuyingCount = 11,
            SellingCount = 12,
            PlayerSelection = 13,
            PlayerCaptain = 14
        }

        public enum TeamPlayerTypeEnum
        {
            Captian = 1,
            ViceCaptian = 2,
            Normal = 3
        }

        public enum SubscriptionEnum
        {
            TripleCaptain = 5,
            DoubleGameWeak = 6,
            BenchBoost = 7,
            Add3MillionsBank = 8,
            Top_11 = 9,
            Gold = 10
        }

        public enum PrivateLeagueEnum
        {
            OfficialLeague = 229
        }

        public enum _365CompetitionsEnum
        {
            Egypt = 552,
            KSA = 649
        }
    }
}
