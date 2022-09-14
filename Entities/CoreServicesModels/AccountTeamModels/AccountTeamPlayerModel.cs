using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.AccountTeamModels
{
    public class AccountTeamPlayerParameters : RequestParameters
    {
        public int Fk_AccountTeam { get; set; }
        public int Fk_Player { get; set; }

    }
    public class AccountTeamPlayerModel : BaseEntity
    {
        public int Fk_AccountTeam { get; set; }
        public int Fk_Player { get; set; }

    }
}
