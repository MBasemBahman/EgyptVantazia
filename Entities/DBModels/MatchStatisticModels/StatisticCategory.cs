namespace Entities.DBModels.MatchStatisticModels
{
    public class StatisticCategory : AuditLookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }

        [DisplayName(nameof(_365_Id))]
        public string _365_Id { get; set; }

        [DisplayName(nameof(StatisticScores))]
        public List<StatisticScore> StatisticScores { get; set; }

        public StatisticCategoryLang StatisticCategoryLang { get; set; }
    }

    public class StatisticCategoryLang : LangEntity<StatisticCategory>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}
