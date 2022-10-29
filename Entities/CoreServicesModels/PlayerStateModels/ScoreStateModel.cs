using Entities.CoreServicesModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PlayerStateModels
{
    public class ScoreStateParameters : RequestParameters
    {
        public List<int> Ids { get; set; }
        public int Fk_GameWeak { get; set; }
        public List<int> Fk_GameWeaks { get; set; }
        public int Fk_Season { get; set; }
        public List<int> Fk_Seasons { get; set; }
        public bool IncludeBestPlayer { get; set; }
    }

    public class ScoreStateModel : AuditLookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }

        [DisplayName(nameof(Description))]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public PlayerModel BestPlayer { get; set; }
    }

    public class ScoreTypeCreateOrEditModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(Description))]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public ScoreStateLangModel ScoreStateLang { get; set; }
    }

    public class ScoreStateLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}
