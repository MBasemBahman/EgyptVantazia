namespace Entities.DBModels.PlayerScoreModels
{
    public class ScoreType : AuditLookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }

        [DisplayName(nameof(Description))]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName(nameof(_365_TypeId))]
        public string _365_TypeId { get; set; }

        [DisplayName(nameof(PlayerGameWeakScores))]
        public IList<PlayerGameWeakScore> PlayerGameWeakScores { get; set; }

        public ScoreTypeLang ScoreTypeLang { get; set; }
    }

    public class ScoreTypeLang : LangEntity<ScoreType>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}
