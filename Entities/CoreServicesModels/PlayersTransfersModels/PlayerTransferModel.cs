using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.EnumData.LogicEnumData;

namespace Entities.CoreServicesModels.PlayersTransfersModels
{
    public class PlayerTransferParameters:RequestParameters
    {
        public int Fk_Player { get; set; }

        public int Fk_AccountTeam { get; set; }

        public int Fk_GameWeak { get; set; }
    }
    public class PlayerTransferModel : AuditEntity
    {
        public int Fk_Player { get; set; }

        public int Fk_AccountTeam { get; set; }

        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(TransferTypeEnum))]
        public TransferTypeEnum TransferTypeEnum { get; set; }

        [DisplayName(nameof(Cost))]
        public int Cost { get; set; }

        [DisplayName(nameof(IsFree))]
        public bool IsFree { get; set; }
    }
}
