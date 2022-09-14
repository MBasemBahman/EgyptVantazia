using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.SeasonModels
{
    public class TeamGameWeakParameters:RequestParameters
    {
        public int Fk_Home { get; set; }

        public int Fk_Away { get; set; }

        public int Fk_GameWeak { get; set; }
    }
    public class TeamGameWeakModel : AuditEntity
    {
        public int Fk_Home { get; set; }

        public int Fk_Away { get; set; }

        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(HomeScore))]
        public int HomeScore { get; set; }

        [DisplayName(nameof(AwayScore))]
        public int AwayScore { get; set; }

        [DisplayName(nameof(StartTime))]
        public DateTime StartTime { get; set; }

        [DisplayName(nameof(_365_MatchId))]
        public string _365_MatchId { get; set; }

        [DisplayName(nameof(_365_MatchUpId))]
        public string _365_MatchUpId { get; set; }
    }
}
