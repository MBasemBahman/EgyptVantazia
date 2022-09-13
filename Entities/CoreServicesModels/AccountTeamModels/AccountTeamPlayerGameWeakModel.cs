using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.SeasonModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class AccountTeamPlayerGameWeakParameters : RequestParameters
    {
        public int Fk_AccountTeamPlayer { get; set; }

        public int Fk_TeamPlayerType { get; set; }
    }
    public class AccountTeamPlayerGameWeakModel : AuditEntity
    {
        public int Fk_AccountTeamPlayer { get; set; }
      
        public int Fk_TeamPlayerType { get; set; }

        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(TrippleCaptain))]
        public bool TrippleCaptain { get; set; }
    }
}
