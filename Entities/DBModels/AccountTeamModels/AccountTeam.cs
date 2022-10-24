using Entities.DBModels.AccountModels;
using Entities.DBModels.PlayersTransfersModels;
using Entities.DBModels.SeasonModels;

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

        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(TotalPoints))]
        public int TotalPoints { get; set; }

        [DisplayName(nameof(TotalMoney))]
        public int TotalMoney { get; set; }

        [DisplayName(nameof(FreeTransfer))]
        public int FreeTransfer { get; set; }

        [DisplayName(nameof(GlobalRanking))]
        public double GlobalRanking { get; set; }

        [DisplayName(nameof(CountryRanking))]
        public double CountryRanking { get; set; }

        [DisplayName(nameof(FavouriteTeamRanking))]
        public double FavouriteTeamRanking { get; set; }

        [DisplayName(nameof(AccountTeamPlayers))]
        public IList<AccountTeamPlayer> AccountTeamPlayers { get; set; }

        [DisplayName(nameof(AccountTeamGameWeaks))]
        public IList<AccountTeamGameWeak> AccountTeamGameWeaks { get; set; }

        [DisplayName(nameof(PlayerTransfers))]
        public IList<PlayerTransfer> PlayerTransfers { get; set; }
    }
}
