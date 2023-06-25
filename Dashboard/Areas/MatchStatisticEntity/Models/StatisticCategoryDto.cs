using Entities.CoreServicesModels.MatchStatisticModels;
using System.ComponentModel;

namespace Dashboard.Areas.MatchStatisticEntity.Models
{
    public class StatisticCategoryFilter : DtParameters
    {
        public int Id { get; set; }
    }
    public class StatisticCategoryDto : StatisticCategoryModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }
    }
}
