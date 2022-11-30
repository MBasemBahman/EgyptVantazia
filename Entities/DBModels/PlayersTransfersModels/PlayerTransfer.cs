using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using static Entities.EnumData.LogicEnumData;

namespace Entities.DBModels.PlayersTransfersModels
{
    public class PlayerTransfer : AuditEntity
    {
        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public Player Player { get; set; }

        [DisplayName(nameof(AccountTeam))]
        [ForeignKey(nameof(AccountTeam))]
        public int Fk_AccountTeam { get; set; }

        [DisplayName(nameof(AccountTeam))]
        public AccountTeam AccountTeam { get; set; }

        [DisplayName(nameof(GameWeak))]
        [ForeignKey(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeak GameWeak { get; set; }

        [DisplayName(nameof(TransferTypeEnum))]
        public TransferTypeEnum TransferTypeEnum { get; set; }

        [DisplayName(nameof(Cost))]
        public double Cost { get; set; }

        [DisplayName(nameof(IsFree))]
        public bool IsFree { get; set; }
    }
}
