using Entities.DBModels.AccountModels;
using Entities.DBModels.PlayersTransfersModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;

namespace Entities.DBModels.AccountTeamModels
{
    public class AccountTeam : AuditImageEntity
    {
        [DisplayName(nameof(Account))]
        [ForeignKey(nameof(Account))]
        public int Fk_Account { get; set; }

        [DisplayName(nameof(Account))]
        public Account Account { get; set; }

        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Season))]
        public Season Season { get; set; }

        [DisplayName(nameof(FavouriteTeam))]
        [ForeignKey(nameof(FavouriteTeam))]
        public int Fk_FavouriteTeam { get; set; }

        [DisplayName(nameof(FavouriteTeam))]
        public Team FavouriteTeam { get; set; }

        [DisplayName(nameof(CommunicationStatus))]
        [ForeignKey(nameof(CommunicationStatus))]
        public int? Fk_CommunicationStatus { get; set; }

        [DisplayName(nameof(CommunicationStatus))]
        public CommunicationStatus CommunicationStatus { get; set; }

        [DisplayName(nameof(CommunicationStatusComment))]
        public string CommunicationStatusComment { get; set; }

        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        #region Calculations

        [DisplayName(nameof(TotalPoints))]
        public int TotalPoints { get; set; }

        [DisplayName(nameof(TotalMoney))]
        public double TotalMoney { get; set; }

        #endregion

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

        [DisplayName(nameof(IsVip))]
        public bool IsVip { get; set; }

        #endregion

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

        [DisplayName(nameof(AccountTeamPlayers))]
        public IList<AccountTeamPlayer> AccountTeamPlayers { get; set; }

        [DisplayName(nameof(AccountTeamGameWeaks))]
        public List<AccountTeamGameWeak> AccountTeamGameWeaks { get; set; }

        [DisplayName(nameof(PlayerTransfers))]
        public IList<PlayerTransfer> PlayerTransfers { get; set; }
    }
}
