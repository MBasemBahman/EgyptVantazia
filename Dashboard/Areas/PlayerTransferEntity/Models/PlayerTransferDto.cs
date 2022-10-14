using Dashboard.Areas.AccountTeamEntity.Models;
using Dashboard.Areas.SeasonEntity.Models;
using Dashboard.Areas.TeamEntity.Models;
using Entities.CoreServicesModels.PlayerTransfersModels;
using System.ComponentModel;
using static Entities.EnumData.LogicEnumData;

namespace Dashboard.Areas.PlayerTransferEntity.Models
{
    public class PlayerTransferDto : PlayerTransferModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(Player))]
        public new PlayerDto Player { get; set; }

        [DisplayName(nameof(AccountTeam))]
        public new AccountTeamDto AccountTeam { get; set; }

        [DisplayName(nameof(GameWeak))]
        public new GameWeakDto GameWeak { get; set; }

    }
}
