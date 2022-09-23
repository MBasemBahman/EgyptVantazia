using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.PlayersTransfersModels;

namespace Entities.DBModels.TeamModels
{
    public class Player : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(_365_PlayerId))]
        public string _365_PlayerId { get; set; }

        [DisplayName(nameof(Team))]
        [ForeignKey(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(Team))]
        public Team Team { get; set; }

        [DisplayName(nameof(PlayerPosition))]
        [ForeignKey(nameof(PlayerPosition))]
        public int Fk_PlayerPosition { get; set; }

        [DisplayName(nameof(PlayerPosition))]
        public PlayerPosition PlayerPosition { get; set; }

        [DisplayName(nameof(PlayerNumber))]
        public string PlayerNumber { get; set; }

        [DisplayName(nameof(PlayerGameWeaks))]
        public IList<PlayerGameWeak> PlayerGameWeaks { get; set; }

        [DisplayName(nameof(AccountTeamPlayers))]
        public IList<AccountTeamPlayer> AccountTeamPlayers { get; set; }

        [DisplayName(nameof(PlayerTransfers))]
        public IList<PlayerTransfer> PlayerTransfers { get; set; }

        [DisplayName(nameof(PlayerPrices))]
        public IList<PlayerPrice> PlayerPrices { get; set; }

        public PlayerLang PlayerLang { get; set; }
    }

    public class PlayerLang : LangEntity<Player>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}
