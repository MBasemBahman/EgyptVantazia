using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.DBModels.MatchStatisticModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Areas.MatchStatisticEntity.Models
{
    public class StatisticScoreFilter : DtParameters
    {
        public int Id { get; set; }

        [DisplayName(nameof(StatisticCategory))]
        public int Fk_StatisticCategory { get; set; }
    }
    public class StatisticScoreDto : StatisticScoreModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(StatisticCategory))]
        public new StatisticCategoryDto StatisticCategory { get; set; }
    }
}
