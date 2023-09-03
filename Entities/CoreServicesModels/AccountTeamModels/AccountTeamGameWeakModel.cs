using Entities.CoreServicesModels.SeasonModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class AccountTeamGameWeakParameters : RequestParameters
    {
        public int Fk_PrivateLeague { get; set; }
        public int Fk_AccountTeam { get; set; }

        public int Fk_GameWeak { get; set; }
        
        public double? PointsFrom { get; set; }
        
        public double? PointsTo { get; set; }

        public bool GetNextGameWeak { get; set; }

        public bool GetPrevGameWeak { get; set; }

        public bool GetCurrentGameWeak { get; set; }

        public int Fk_Account { get; set; }

        public int Fk_Season { get; set; }

        public string _365_GameWeakId { get; set; }

        public int GameWeakFrom { get; set; }

        public int GameWeakTo { get; set; }

        public bool? BenchBoost { get; set; }
        public bool? FreeHit { get; set; }
        public bool? WildCard { get; set; }
        public bool? DoubleGameWeak { get; set; }
        public bool? Top_11 { get; set; }
        public bool? TripleCaptain { get; set; }
        public bool? TwiceCaptain { get; set; }
        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }
        
        [DisplayName(nameof(AccountFullName))]
        public string AccountFullName { get; set; }

        [DisplayName(nameof(AccountUserName))]
        public string AccountUserName { get; set; }

        public string DashboardSearch { get; set; }

        public List<int> Fk_Players { get; set; }

        public List<int> Fk_Teams { get; set; }

        public bool IncludeNextAndPrevGameWeek { get; set; }

        public bool? UseCards { get; set; }
    }

    public class AccountTeamGameWeakModel : AuditEntity
    {
        [DisplayName(nameof(AccountTeam))]
        public int Fk_AccountTeam { get; set; }

        [DisplayName(nameof(AccountTeam))]
        public AccountTeamModel AccountTeam { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeakModel GameWeak { get; set; }

        [DisplayName(nameof(PrevGameWeak))]
        public GameWeakModel PrevGameWeak { get; set; }

        [DisplayName(nameof(NextGameWeak))]
        public GameWeakModel NextGameWeak { get; set; }

        [DisplayName(nameof(BenchBoost))]
        public bool BenchBoost { get; set; }

        [DisplayName(nameof(AvailableBenchBoost))]
        public bool AvailableBenchBoost { get; set; }

        [DisplayName(nameof(FreeHit))]
        public bool FreeHit { get; set; }

        [DisplayName(nameof(AvailableFreeHit))]
        public bool AvailableFreeHit { get; set; }

        [DisplayName(nameof(WildCard))]
        public bool WildCard { get; set; }

        [DisplayName(nameof(AvailableWildCard))]
        public bool AvailableWildCard { get; set; }

        [DisplayName(nameof(SeasonTotalPoints))]
        public double SeasonTotalPoints { get; set; }

        [DisplayName(nameof(TotalPoints))]
        public int? TotalPoints { get; set; }

        [DisplayName(nameof(PrevPoints))]
        public int PrevPoints { get; set; }

        public bool IsUp => TotalPoints > PrevPoints;

        [DisplayName(nameof(DoubleGameWeak))]
        public bool DoubleGameWeak { get; set; }

        [DisplayName(nameof(AvailableDoubleGameWeak))]
        public bool AvailableDoubleGameWeak { get; set; }

        [DisplayName(nameof(Top_11))]
        public bool Top_11 { get; set; }

        [DisplayName(nameof(AvailableTop_11))]
        public bool AvailableTop_11 { get; set; }

        [DisplayName(nameof(TripleCaptain))]
        public bool TripleCaptain { get; set; }

        [DisplayName(nameof(TwiceCaptain))]
        public bool TwiceCaptain { get; set; }

        [DisplayName(nameof(TansfarePoints))]
        public int TansfarePoints { get; set; }

        [DisplayName(nameof(TansfareCount))]
        public int TansfareCount { get; set; }

        [DisplayName(nameof(BenchPoints))]
        public int BenchPoints { get; set; }

        [DisplayName(nameof(SeasonGlobalRanking))]
        public double SeasonGlobalRanking { get; set; }

        [DisplayName(nameof(SeasonGoldSubscriptionRanking))]
        public double? SeasonGoldSubscriptionRanking { get; set; }

        [DisplayName(nameof(SeasonUnSubscriptionRanking))]
        public double? SeasonUnSubscriptionRanking { get; set; }

        [DisplayName(nameof(GlobalRanking))]
        public double GlobalRanking { get; set; }

        [DisplayName(nameof(GlobalRankingUpdatedAt))]
        public DateTime? GlobalRankingUpdatedAt { get; set; }

        [DisplayName(nameof(CountryRanking))]
        public double CountryRanking { get; set; }

        [DisplayName(nameof(CountryRankingUpdatedAt))]
        public DateTime? CountryRankingUpdatedAt { get; set; }

        [DisplayName(nameof(FavouriteTeamRanking))]
        public double FavouriteTeamRanking { get; set; }

        [DisplayName(nameof(FavouriteTeamRankingUpdatedAt))]
        public DateTime? FavouriteTeamRankingUpdatedAt { get; set; }
    }

    public class AccountTeamGameWeakModelForCalc
    {
        public int Id { get; set; }

        public bool FreeHit { get; set; }

        [DisplayName(nameof(BenchBoost))]
        public bool BenchBoost { get; set; }

        [DisplayName(nameof(WildCard))]
        public bool WildCard { get; set; }

        [DisplayName(nameof(DoubleGameWeak))]
        public bool DoubleGameWeak { get; set; }

        [DisplayName(nameof(Top_11))]
        public bool Top_11 { get; set; }

        [DisplayName(nameof(TripleCaptain))]
        public bool TripleCaptain { get; set; }

        [DisplayName(nameof(TwiceCaptain))]
        public bool TwiceCaptain { get; set; }
    }
}
