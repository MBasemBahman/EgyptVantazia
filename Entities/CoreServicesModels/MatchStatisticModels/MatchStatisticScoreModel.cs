using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.MatchStatisticModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.MatchStatisticModels
{
    public class MatchStatisticScoreParameters : RequestParameters
    {
        public List<int> Fk_Teams { get; set; }

        public List<int> Fk_StatisticScores { get; set; }

        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        public int Fk_TeamGameWeak { get; set; }

        public int Fk_Team { get; set; }

        [DisplayName(nameof(_365_MatchId))]
        public string _365_MatchId { get; set; }

        [DisplayName(nameof(IsCanNotEdit))]
        public bool? IsCanNotEdit { get; set; }
        public DateTime? CreatedAtFrom { get; set; }
        public DateTime? CreatedAtTo { get; set; }
    }
    public class MatchStatisticScoreModel : AuditEntity
    {
        [DisplayName(nameof(StatisticScore))]
        [ForeignKey(nameof(StatisticScore))]
        public int Fk_StatisticScore { get; set; }

        [DisplayName(nameof(StatisticScore))]
        public StatisticScoreModel StatisticScore { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        [ForeignKey(nameof(TeamGameWeak))]
        public int Fk_TeamGameWeak { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        public TeamGameWeakModel TeamGameWeak { get; set; }

        [DisplayName(nameof(Team))]
        [ForeignKey(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(Team))]
        public TeamModel Team { get; set; }

        [DisplayName(nameof(Value))]
        public string Value { get; set; }

        [DisplayName(nameof(ValuePercentage))]
        public double ValuePercentage { get; set; }

        [DisplayName(nameof(IsCanNotEdit))]
        public bool IsCanNotEdit { get; set; }
    }

    public class MatchStatisticScoreCreateOrEditModel
    {
        [DisplayName(nameof(StatisticScore))]
        public int Fk_StatisticScore { get; set; }

        [DisplayName(nameof(StatisticCategory))]
        public int Fk_StatisticCategory { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        public int Fk_TeamGameWeak { get; set; }

        [DisplayName(nameof(Team))]
        [ForeignKey(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(Value))]
        public string Value { get; set; }

        [DisplayName(nameof(ValuePercentage))]
        public double ValuePercentage { get; set; }

        [DisplayName(nameof(IsCanNotEdit))]
        public bool IsCanNotEdit { get; set; }
    }
}
