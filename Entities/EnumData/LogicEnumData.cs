namespace Entities.EnumData
{
    public static class LogicEnumData
    {
        public enum AppViewEnum
        {
            More = 1,
            States = 2,
            Latest = 3,
            Fantasy = 4
        }

        public enum NewsTypeEnum
        {
            Common = 1
        }

        public enum TransferTypeEnum
        {
            Buying = 1,
            Selling = 2
        }

        public enum PyamentTypeEnum
        {
            Credit = 1,
            Wallet = 2,
            Kiosk = 3
        }

        public enum CardTypeEnum
        {
            BenchBoost = 1,
            FreeHit = 2,
            WildCard = 3,
            DoubleGameWeak = 4,
            Top_11 = 5,
            TripleCaptain = 6
        }

        public enum PlayMinutesEnum
        {
            NotPlayed,
            Played,
            PlayMoreThan60Min
        }

        public enum NotificationOpenTypeEnum
        {
            Home,
            News,
            PlayerProfile,
            MatchProfile,
            DeadLine
        }
    }
}
