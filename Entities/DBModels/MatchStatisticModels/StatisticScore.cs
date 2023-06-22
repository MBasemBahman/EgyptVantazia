namespace Entities.DBModels.MatchStatisticModels
{
    public class StatisticScore : AuditLookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }

        [DisplayName(nameof(_365_Id))]
        public string _365_Id { get; set; }

        [DisplayName(nameof(StatisticCategory))]
        [ForeignKey(nameof(StatisticCategory))]
        public int Fk_StatisticCategory { get; set; }

        [DisplayName(nameof(StatisticCategory))]
        public StatisticCategory StatisticCategory { get; set; }

        [DisplayName(nameof(MatchStatisticScores))]
        public List<MatchStatisticScore> MatchStatisticScores { get; set; }

        public StatisticScoreLang StatisticScoreLang { get; set; }
    }

    public class StatisticScoreLang : LangEntity<StatisticScore>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}
