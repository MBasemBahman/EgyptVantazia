using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerMarkModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PlayerMarkModels
{
    public class PlayerMarkParameters : RequestParameters
    {
        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Mark))]
        [ForeignKey(nameof(Mark))]
        public int Fk_Mark { get; set; }

        [DisplayName(nameof(IsValid))]
        public bool? IsValid { get; set; }

        [DisplayName(nameof(Team))]
        public List<int> Fk_Teams { get; set; }

        [DisplayName(nameof(Player))]
        public List<int> Fk_Players { get; set; }

        public string SearchBy { get; set; }
    }

    public class PlayerMarkModel : AuditEntity
    {
        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public PlayerModel Player { get; set; }

        [DisplayName(nameof(Mark))]
        [ForeignKey(nameof(Mark))]
        public int Fk_Mark { get; set; }

        [DisplayName(nameof(Mark))]
        public MarkModel Mark { get; set; }

        [DisplayName(nameof(Count))]
        public int? Count { get; set; }

        [DisplayName(nameof(Used))]
        public int? Used { get; set; }

        [DisplayName(nameof(DateTo))]
        public DateTime? DateTo { get; set; }
        [DisplayName(nameof(Percent))]
        public int Percent { get; set; }

        [DisplayName(nameof(Notes))]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
        public bool IsValid => DateTo >= DateTime.UtcNow;
    }

    public class PlayerMarkCreateOrEditModel
    {
        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Mark))]
        [ForeignKey(nameof(Mark))]
        public int Fk_Mark { get; set; }

        [DisplayName(nameof(Value))]
        public string Value { get; set; }

        [DisplayName(nameof(Count))]
        public int? Count { get; set; }

        [DisplayName(nameof(Used))]
        public int? Used { get; set; }

        [DisplayName(nameof(DateTo))]
        public DateTime? DateTo { get; set; }

        [DisplayName(nameof(Percent))]
        public int Percent { get; set; }

        [DisplayName(nameof(Notes))]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [DisplayName(nameof(Fk_TeamGameWeaks))]
        public List<int> Fk_TeamGameWeaks { get; set; }

        [DisplayName(nameof(Fk_PlayerMarkReasonMatches))]
        public List<int> Fk_PlayerMarkReasonMatches { get; set; }
    }
}
