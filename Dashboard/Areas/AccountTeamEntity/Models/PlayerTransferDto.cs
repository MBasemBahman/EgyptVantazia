using Dashboard.Areas.AccountEntity.Models;
using Dashboard.Areas.PlayerTransferEntity.Models;
using Dashboard.Areas.SeasonEntity.Models;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.SeasonModels;
using System.ComponentModel;
using Entities.CoreServicesModels.PlayerTransfersModels;
using Entities.DBModels.TeamModels;
using Entities.EnumData;

namespace Dashboard.Areas.AccountTeamEntity.Models
{
    public class PlayerTransferFilter : DtParameters
    {
        public int Id { get; set; }
        public int Fk_Player { get; set; }

        public int Fk_AccountTeam { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }
        [DisplayName(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(IsFree))]
        public bool? IsFree { get; set; }

        public int Fk_Account { get; set; }

        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }
        
        [DisplayName(nameof(TransferTypeEnum))]
        public LogicEnumData.TransferTypeEnum? TransferTypeEnum { get; set; }
    }
}
