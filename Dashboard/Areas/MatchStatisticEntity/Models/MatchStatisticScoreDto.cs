using Dashboard.Areas.SeasonEntity.Models;
using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.MatchStatisticModels;
using Entities.DBModels.SeasonModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Areas.MatchStatisticEntity.Models
{
    public class MatchStatisticScoreFilter : DtParameters
    {
        public int Id { get; set; }
        [DisplayName(nameof(StatisticCategory))]
        public int Fk_StatisticCategory { get; set; }
        public int Fk_TeamGameWeak { get; set; }
        [DisplayName("Teams")]
        public List<int> Fk_Teams { get; set; }

        [DisplayName("StatisticScores")]
        public List<int> Fk_StatisticScores { get; set; }

        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(_365_MatchId))]
        public string _365_MatchId { get; set; }

        [DisplayName(nameof(IsCanNotEdit))]
        public bool? IsCanNotEdit { get; set; }

        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }
    }
    public class MatchStatisticScoreDto : MatchStatisticScoreModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(StatisticScore))]
        public new StatisticScoreDto StatisticScore { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        public new TeamGameWeakDto TeamGameWeak { get; set; }

    }
}
