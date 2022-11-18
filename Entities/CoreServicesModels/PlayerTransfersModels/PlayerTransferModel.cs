using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.RequestFeatures;
using System.Collections;
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

    public class PlayerTransferBulkCreateModel
    {
        public IList<PlayerTransferSellModel> SellPlayers { get; set; }

        public IList<PlayerTransferBuyModel> BuyPlayers { get; set; }
    }

    public class SellPlayerModel
    {
        public int Id { get; set; }
        public int Fk_TeamPlayerType { get; set; }
        public bool IsPrimary { get; set; }
        public int Order { get; set; }
    }

    public class PlayerTransferSellModel
    {
        public int Fk_Player { get; set; }
    }

    public class PlayerTransferBuyModel
    {
        public int Fk_Player { get; set; }

        [DisplayName(nameof(IsFree))]
        public bool IsFree { get; set; }
    }
}
