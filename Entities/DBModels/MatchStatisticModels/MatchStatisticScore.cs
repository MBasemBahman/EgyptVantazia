using Entities.DBModels.SeasonModels;

namespace Entities.DBModels.MatchStatisticModels
{
    public class MatchStatisticScore : AuditEntity
    {
        [DisplayName(nameof(StatisticScore))]
        [ForeignKey(nameof(StatisticScore))]
        public int Fk_StatisticScore { get; set; }

        [DisplayName(nameof(StatisticScore))]
        public StatisticScore StatisticScore { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        [ForeignKey(nameof(TeamGameWeak))]
        public int Fk_TeamGameWeak { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        public TeamGameWeak TeamGameWeak { get; set; }

        [DisplayName(nameof(Value))]
        public string Value { get; set; }

        [DisplayName(nameof(ValuePercentage))]
        public double ValuePercentage { get; set; }

        [DisplayName(nameof(IsCanNotEdit))]
        public bool IsCanNotEdit { get; set; }
    }
}
