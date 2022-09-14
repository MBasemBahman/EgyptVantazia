using Entities.DBModels.AccountModels;
using Entities.DBModels.PrivateLeagueModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.PrivateLeagueModels
{
    public class PrivateLeagueMemberParameters : RequestParameters
    {
        public int Fk_Account { get; set; }

        public int Fk_PrivateLeague { get; set; }
    }
    public class PrivateLeagueMemberModel : AuditEntity
    {
        public int Fk_Account { get; set; }

        public int Fk_PrivateLeague { get; set; }

        [DisplayName(nameof(IsAdmin))]
        public bool IsAdmin { get; set; }
    }
}
