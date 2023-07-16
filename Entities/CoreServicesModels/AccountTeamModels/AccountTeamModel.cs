using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class AccountTeamParameters : RequestParameters
    {
        public List<int> Fk_AccountTeams { get; set; }
        public int Fk_Account { get; set; }
        public int Fk_User { get; set; }
        public int Fk_Season { get; set; }
        public int Fk_GameWeak { get; set; }

        public int Fk_AccountTeamForOrder { get; set; }

        public double? PointsFrom { get; set; }
        public double? PointsTo { get; set; }
        public bool? CurrentSeason { get; set; }

        public int Fk_PrivateLeague { get; set; }

        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }

        [DisplayName(nameof(AccountFullName))]
        public string AccountFullName { get; set; }

        [DisplayName(nameof(AccountUserName))]
        public string AccountUserName { get; set; }

        public bool IncludeGameWeakPoints { get; set; }

        public int Fk_Country { get; set; }

        public int Fk_FavouriteTeam { get; set; }

        public int? FromTotalPoints { get; set; }
        public int? FromGlobalRanking { get; set; }
        public int? FromGoldSubscriptionRanking { get; set; }
        public int? FromUnSubscriptionRanking { get; set; }

        public string DashboardSearch { get; set; }

        public int? FromCurrentGameWeakPoints { get; set; }

        public bool IncludeTotalPoints { get; set; }

        public bool IncludeNextAndPrevGameWeek { get; set; }

        public bool GetMonthPlayer { get; set; }

        public int From_365_GameWeakIdValue { get; set; }
        public int To_365_GameWeakIdValue { get; set; }

        public int _365_CompetitionsId { get; set; }
    }

    public class AccountTeamModel : AuditImageEntity
    {
        [DisplayName(nameof(Account))]
        public int Fk_Account { get; set; }

        [DisplayName(nameof(Account))]
        public AccountModel Account { get; set; }

        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Season))]
        public SeasonModel Season { get; set; }

        [DisplayName(nameof(FavouriteTeam))]
        [ForeignKey(nameof(FavouriteTeam))]
        public int Fk_FavouriteTeam { get; set; }

        [DisplayName(nameof(FavouriteTeam))]
        public TeamModel FavouriteTeam { get; set; }

        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(TotalPoints))]
        public int TotalPoints { get; set; }

        [DisplayName(nameof(TotalMoney))]
        public double TotalMoney { get; set; }

        [DisplayName(nameof(IsVip))]
        public bool IsVip { get; set; }

        #region Ranking

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

        [DisplayName(nameof(GoldSubscriptionRanking))]
        public double? GoldSubscriptionRanking { get; set; }

        [DisplayName(nameof(GoldSubscriptionUpdatedAt))]
        public DateTime? GoldSubscriptionUpdatedAt { get; set; }

        [DisplayName(nameof(UnSubscriptionRanking))]
        public double? UnSubscriptionRanking { get; set; }

        [DisplayName(nameof(UnSubscriptionUpdatedAt))]
        public DateTime? UnSubscriptionUpdatedAt { get; set; }

        #endregion

        public int CurrentGameWeakTansfarePoints { get; set; }

        public int CurrentGameWeakPoints { get; set; }

        public int Fk_AcountTeamGameWeek { get; set; }

        public int PrevGameWeakPoints { get; set; }

        public bool IsUp => CurrentGameWeakPoints > PrevGameWeakPoints;

        public double AverageGameWeakPoints { get; set; }

        #region Ranking

        [DisplayName(nameof(CurrentGameWeakGlobalRanking))]
        public double CurrentGameWeakGlobalRanking { get; set; }

        [DisplayName(nameof(CurrentGameWeakCountryRanking))]
        public double CurrentGameWeakCountryRanking { get; set; }

        [DisplayName(nameof(CurrentGameWeakFavouriteTeamRanking))]
        public double CurrentGameWeakFavouriteTeamRanking { get; set; }

        #endregion

        public AccountTeamGameWeakModel BestAccountTeamGameWeak { get; set; }

        public int TransferCount { get; set; }

        public int FreeTransferCount { get; set; }

        public double TotalTeamPrice { get; set; }

        public int AveragePoints { get; set; }

        public int MaxPoints { get; set; }

        public int PlayersCount { get; set; }

        #region Cards

        [DisplayName(nameof(BenchBoost))]
        [DefaultValue(0)]
        public int BenchBoost { get; set; }

        [DisplayName(nameof(FreeHit))]
        [DefaultValue(0)]
        public int FreeHit { get; set; }

        [DisplayName(nameof(WildCard))]
        [DefaultValue(0)]
        public int WildCard { get; set; }

        [DisplayName(nameof(DoubleGameWeak))]
        [DefaultValue(0)]
        public int DoubleGameWeak { get; set; }

        [DisplayName(nameof(TwiceCaptain))]
        [DefaultValue(0)]
        public int TwiceCaptain { get; set; }

        [DisplayName(nameof(Top_11))]
        [DefaultValue(0)]
        public int Top_11 { get; set; }

        [DisplayName(nameof(FreeTransfer))]
        [DefaultValue(0)]
        public int FreeTransfer { get; set; }

        [DisplayName(nameof(TripleCaptain))]
        [DefaultValue(0)]
        public int TripleCaptain { get; set; }

        #endregion

        public int AccounTeamGameWeakCount { get; set; }

        [DisplayName(nameof(HaveGoldSubscription))]
        public bool HaveGoldSubscription { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeakModel GameWeak { get; set; }

        [DisplayName(nameof(PrevGameWeak))]
        public GameWeakModel PrevGameWeak { get; set; }

        [DisplayName(nameof(NextGameWeak))]
        public GameWeakModel NextGameWeak { get; set; }
    }

    public class AccountTeamModelForCalc
    {
        public int Id { get; set; }

        [DisplayName(nameof(TotalMoney))]
        public double TotalMoney { get; set; }

        #region Cards

        [DisplayName(nameof(BenchBoost))]
        [DefaultValue(0)]
        public int BenchBoost { get; set; }

        [DisplayName(nameof(FreeHit))]
        [DefaultValue(0)]
        public int FreeHit { get; set; }

        [DisplayName(nameof(WildCard))]
        [DefaultValue(0)]
        public int WildCard { get; set; }

        [DisplayName(nameof(DoubleGameWeak))]
        [DefaultValue(0)]
        public int DoubleGameWeak { get; set; }

        [DisplayName(nameof(Top_11))]
        [DefaultValue(0)]
        public int Top_11 { get; set; }

        [DisplayName(nameof(FreeTransfer))]
        [DefaultValue(0)]
        public int FreeTransfer { get; set; }

        [DisplayName(nameof(TripleCaptain))]
        [DefaultValue(0)]
        public int TripleCaptain { get; set; }

        [DisplayName(nameof(TwiceCaptain))]
        [DefaultValue(0)]
        public int TwiceCaptain { get; set; }

        #endregion
    }

    public class AccountTeamCreateOrEditModel
    {
        [DisplayName(nameof(Account))]
        [ForeignKey(nameof(Account))]
        public int Fk_Account { get; set; }

        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName("FavouriteTeam")]
        public int Fk_FavouriteTeam { get; set; }

        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(ImageUrl))]
        public string ImageUrl { get; set; }

        [DisplayName(nameof(StorageUrl))]
        public string StorageUrl { get; set; }

        [DisplayName(nameof(TotalPoints))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public int TotalPoints { get; set; }

        [DisplayName(nameof(TotalMoney))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public int TotalMoney { get; set; }

        #region Cards

        [DisplayName(nameof(BenchBoost))]
        [DefaultValue(0)]
        public int BenchBoost { get; set; }

        [DisplayName(nameof(FreeHit))]
        [DefaultValue(0)]
        public int FreeHit { get; set; }

        [DisplayName(nameof(WildCard))]
        [DefaultValue(0)]
        public int WildCard { get; set; }

        [DisplayName(nameof(DoubleGameWeak))]
        [DefaultValue(0)]
        public int DoubleGameWeak { get; set; }

        [DisplayName(nameof(TwiceCaptain))]
        [DefaultValue(0)]
        public int TwiceCaptain { get; set; }

        [DisplayName(nameof(Top_11))]
        [DefaultValue(0)]
        public int Top_11 { get; set; }

        [DisplayName(nameof(FreeTransfer))]
        [DefaultValue(0)]
        public int FreeTransfer { get; set; }

        [DisplayName(nameof(TripleCaptain))]
        [DefaultValue(0)]
        public int TripleCaptain { get; set; }

        #endregion

        #region Ranking

        [DisplayName(nameof(GlobalRanking))]
        public double GlobalRanking { get; set; }

        [DisplayName(nameof(CountryRanking))]
        public double CountryRanking { get; set; }

        [DisplayName(nameof(FavouriteTeamRanking))]
        public double FavouriteTeamRanking { get; set; }

        [DisplayName(nameof(GoldSubscriptionRanking))]
        public double? GoldSubscriptionRanking { get; set; }

        [DisplayName(nameof(UnSubscriptionRanking))]
        public double? UnSubscriptionRanking { get; set; }
        #endregion
    }

    public class AccountTeamCreateModel
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(ImageFile))]
        public IFormFile ImageFile { get; set; }

        public int Fk_FavouriteTeam { get; set; }
    }

    public class AccountTeamCustemClac
    {
        public int? TotalPoints { get; set; }

        public int BenchPoints { get; set; }

        public int PrevPoints { get; set; }

        public List<AccountTeamPlayerGameWeak> Players { get; set; }
    }

    public class RankingModel
    {
        public int Id { get; set; }

        public int Rank { get; set; }
    }

    public class AccountTeamRanking
    {
        public int Id { get; set; }
        public int Fk_Country { get; set; }
        public int Fk_FavouriteTeam { get; set; }
        public int CountryRanking { get; set; }
        public int FavouriteTeamRanking { get; set; }
        public int GlobalRanking { get; set; }
        public int TotalPoints { get; set; }
    }

    public class AccountTeamEditCardModel
    {
        [DisplayName(nameof(BenchBoost))]
        [DefaultValue(0)]
        public int BenchBoost { get; set; }

        [DisplayName(nameof(FreeHit))]
        [DefaultValue(0)]
        public int FreeHit { get; set; }

        [DisplayName(nameof(WildCard))]
        [DefaultValue(0)]
        public int WildCard { get; set; }

        [DisplayName(nameof(DoubleGameWeak))]
        [DefaultValue(0)]
        public int DoubleGameWeak { get; set; }

        [DisplayName(nameof(Top_11))]
        [DefaultValue(0)]
        public int Top_11 { get; set; }

        [DisplayName(nameof(TwiceCaptain))]
        [DefaultValue(0)] 
        public int TwiceCaptain { get; set; }

        [DisplayName(nameof(FreeTransfer))]
        [DefaultValue(0)]
        public int FreeTransfer { get; set; }

        [DisplayName(nameof(TripleCaptain))]
        [DefaultValue(0)]
        public int TripleCaptain { get; set; }
    }

    public class AccountTeamsUpdateCards : AccountTeamUpdateCards
    {
        [DisplayName(nameof(Fk_AccounTeams))]
        public List<int> Fk_AccounTeams { get; set; }
    }

    public class AccountTeamUpdateCards : AccountTeamEditCardModel
    {
        [DisplayName(nameof(Fk_AccounTeam))]
        public int Fk_AccounTeam { get; set; }
    }

}
