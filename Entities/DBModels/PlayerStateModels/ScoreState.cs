using Entities.DBModels.PlayerScoreModels;

namespace Entities.DBModels.PlayerStateModels
{
    public class ScoreState : AuditLookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }

        [DisplayName(nameof(Description))]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName(nameof(PlayerSeasonScoreStates))]
        public IList<PlayerSeasonScoreState> PlayerSeasonScoreStates { get; set; }

        [DisplayName(nameof(PlayerGameWeakScoreStates))]
        public IList<PlayerGameWeakScoreState> PlayerGameWeakScoreStates { get; set; }

        public ScoreStateLang ScoreStateLang { get; set; }
    }

    public class ScoreStateLang : LangEntity<ScoreState>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}
