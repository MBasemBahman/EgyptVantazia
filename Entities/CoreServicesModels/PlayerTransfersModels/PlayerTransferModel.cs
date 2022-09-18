using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.RequestFeatures;
using static Entities.EnumData.LogicEnumData;

namespace Entities.CoreServicesModels.PlayerTransfersModels
{
    public class PlayerTransferParameters : RequestParameters
    {
        public int Fk_Player { get; set; }

        public int Fk_AccountTeam { get; set; }

        public int Fk_GameWeak { get; set; }

        public bool? IsFree { get; set; }

        public int Fk_Account { get; set; }

        public int Fk_Season { get; set; }
    }

    public class PlayerTransferModel : AuditEntity
    {
        [DisplayName(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public PlayerModel Player { get; set; }

        [DisplayName(nameof(AccountTeam))]
        public int Fk_AccountTeam { get; set; }

        [DisplayName(nameof(AccountTeam))]
        public AccountTeamModel AccountTeam { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeakModel GameWeak { get; set; }

        [DisplayName(nameof(TransferTypeEnum))]
        public TransferTypeEnum TransferTypeEnum { get; set; }

        [DisplayName(nameof(Cost))]
        public int Cost { get; set; }

        [DisplayName(nameof(IsFree))]
        public bool IsFree { get; set; }
    }

    public class PlayerTransferCreateModel
    {
        public int Fk_Player { get; set; }

        public int Fk_GameWeak { get; set; }

        public TransferTypeEnum TransferTypeEnum { get; set; }

        [DisplayName(nameof(Cost))]
        public int Cost { get; set; }

        [DisplayName(nameof(IsFree))]
        public bool IsFree { get; set; }
    }
}
