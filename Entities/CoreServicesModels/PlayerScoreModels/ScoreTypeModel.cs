using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PlayerScoreModels
{
    public class ScoreTypeParameters : RequestParameters
    {
        public List<int> Ids { get; set; }

        [DisplayName(nameof(_365_TypeId))]
        public string _365_TypeId { get; set; }

        [DisplayName(nameof(HavePoints))]
        public bool? HavePoints { get; set; }

        [DisplayName(nameof(IsEvent))]
        public bool? IsEvent { get; set; }

        public int Fk_Season { get; set; }

        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(IsCanNotEdit))]
        public bool? IsCanNotEdit { get; set; }

        public bool IncludeTypeName { get; set; }
    }

    public class ScoreTypeModel : AuditLookUpEntity
    {
        [DisplayName($"{nameof(Name)}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public new string Name { get; set; }

        [DisplayName(nameof(Description))]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName(nameof(_365_TypeId))]
        public string _365_TypeId { get; set; }

        [DisplayName(nameof(_365_EventTypeId))]
        public string _365_EventTypeId { get; set; }

        [DisplayName(nameof(HavePoints))]
        public bool HavePoints { get; set; }

        [DisplayName(nameof(IsEvent))]
        public bool IsEvent { get; set; }

        [DisplayName(nameof(IsCanNotEdit))]
        public bool IsCanNotEdit { get; set; }
    }

    public class ScoreTypeCreateOrEditModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(Description))]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName(nameof(_365_TypeId))]
        public string _365_TypeId { get; set; }

        [DisplayName(nameof(HavePoints))]
        public bool HavePoints { get; set; }

        [DisplayName(nameof(IsEvent))]
        public bool IsEvent { get; set; }

        [DisplayName(nameof(_365_EventTypeId))]
        public string _365_EventTypeId { get; set; }

        [DisplayName(nameof(Fk_Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(IsCanNotEdit))]
        public bool IsCanNotEdit { get; set; }

        public ScoreTypeLangModel ScoreTypeLang { get; set; }
    }

    public class ScoreTypeLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}
