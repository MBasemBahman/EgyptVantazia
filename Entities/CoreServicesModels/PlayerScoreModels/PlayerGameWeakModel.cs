using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.PlayerScoreModels
{
    public class PlayerGameWeakParameters:RequestParameters
    {
        public int Fk_GameWeak { get; set; }
        public int Fk_Player { get; set; }

    }
    public class PlayerGameWeakModel : AuditEntity
    {
        public int Fk_GameWeak { get; set; }

        public int Fk_Player { get; set; }

    }
}
