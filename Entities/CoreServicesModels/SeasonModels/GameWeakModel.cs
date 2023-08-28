using Entities.DBModels.SeasonModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.SeasonModels
{
    public class GameWeakParameters : RequestParameters
    {
        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(_365_GameWeakId))]
        public string _365_GameWeakId { get; set; }

        [DisplayName(nameof(IsCurrent))]
        public bool? IsCurrent { get; set; }

        [DisplayName(nameof(IsCurrent))]
        public bool? IsCurrentSeason { get; set; }

        [DisplayName(nameof(_365_CompetitionsId))]
        public int _365_CompetitionsId { get; set; }

        [DisplayName(nameof(IsDelayed))]
        public bool? IsDelayed { get; set; }

        [DisplayName(nameof(BiggerThanWeak))]
        public int? BiggerThanWeak { get; set; }

        [DisplayName(nameof(LowerThanWeak))]
        public int? LowerThanWeak { get; set; }
        public DateTime? Deadline { get; set; }

        public DateTime? DeadlineFrom { get; set; }

        public DateTime? DeadlineTo { get; set; }

        [DisplayName(nameof(IsNext))]
        public bool? IsNext { get; set; }

        [DisplayName(nameof(IsPrev))]
        public bool? IsPrev { get; set; }

        public int GameWeakFrom { get; set; }

        public int GameWeakTo { get; set; }
    }

    public class GameWeakModel : AuditEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(OtherName))]
        public string OtherName { get; set; }

        [DisplayName(nameof(_365_GameWeakId))]
        public string _365_GameWeakId { get; set; }

        [DisplayName(nameof(_365_GameWeakIdValue))]
        public int _365_GameWeakIdValue { get; set; }

        [DisplayName(nameof(_365_GameWeakId_Parsed))]
        public int? _365_GameWeakId_Parsed => string.IsNullOrWhiteSpace(_365_GameWeakId) ? null : int.Parse(_365_GameWeakId);

        [DisplayName(nameof(IsCurrent))]
        public bool IsCurrent { get; set; }

        [DisplayName(nameof(IsNext))]
        public bool IsNext { get; set; }

        [DisplayName(nameof(IsPrev))]
        public bool IsPrev { get; set; }

        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Season))]
        public SeasonModel Season { get; set; }

        [DisplayName(nameof(Deadline))]
        public DateTime? Deadline { get; set; }

        [DisplayName(nameof(JobId))]
        public string JobId { get; set; }

        public int Order { get; set; }

    }

    public class GameWeakModelForCalc
    {
        public int Id { get; set; }

        public int Fk_Season { get; set; }

        [DisplayName(nameof(_365_CompetitionsId))]
        public string _365_CompetitionsId { get; set; }

        public DateTime? Deadline { get; set; }

        [DisplayName(nameof(_365_GameWeakId))]
        public string _365_GameWeakId { get; set; }

        [DisplayName(nameof(_365_GameWeakIdValue))]
        public int _365_GameWeakIdValue { get; set; }

        [DisplayName(nameof(_365_GameWeakId_Parsed))]
        public int? _365_GameWeakId_Parsed => string.IsNullOrWhiteSpace(_365_GameWeakId) ? null : int.Parse(_365_GameWeakId);
    }

    public class GameWeakCreateOrEditModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string NameEn { get; set; }

        [DisplayName(nameof(_365_GameWeakId))]
        public string _365_GameWeakId { get; set; }

        [DisplayName(nameof(IsCurrent))]
        public bool IsCurrent { get; set; }

        [DisplayName(nameof(IsNext))]
        public bool IsNext { get; set; }

        [DisplayName(nameof(IsPrev))]
        public bool IsPrev { get; set; }

        [DisplayName(nameof(Deadline))]
        public DateTime? Deadline { get; set; }

        public int Id { get; set; }
    }
}
