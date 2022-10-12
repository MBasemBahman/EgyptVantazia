using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.SeasonModels;
using Entities.Extensions;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.SeasonModels
{
    public class TeamGameWeakParameters : RequestParameters
    {
        public int Fk_Home { get; set; }

        public int Fk_Away { get; set; }

        public int Fk_Season { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(_365_MatchId))]
        public string _365_MatchId { get; set; }

        [DisplayName(nameof(FromTime))]
        public DateTime? FromTime { get; set; }

        [DisplayName(nameof(ToTime))]
        public DateTime? ToTime { get; set; }
    }

    public class TeamGameWeakModel : AuditEntity
    {
        [DisplayName(nameof(Home))]
        public int Fk_Home { get; set; }

        [DisplayName(nameof(Home))]
        public TeamModel Home { get; set; }

        [DisplayName(nameof(Away))]
        public int Fk_Away { get; set; }

        [DisplayName(nameof(Away))]
        public TeamModel Away { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeakModel GameWeak { get; set; }

        [DisplayName(nameof(HomeScore))]
        public int HomeScore { get; set; }

        [DisplayName(nameof(AwayScore))]
        public int AwayScore { get; set; }

        [DisplayName(nameof(StartTime))]
        public DateTime StartTime { get; set; }

        [DisplayName(nameof(StartTime))]
        public string StartTimeString => StartTime.ToShortDateTimeString();

        [DisplayName(nameof(_365_MatchId))]
        public string _365_MatchId { get; set; }

        [DisplayName(nameof(IsEnded))]
        public bool IsEnded { get; set; }
    }

    public class TeamGameWeakCreateOrEditModel
    {
        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName("Home")]
        public int Fk_Home { get; set; }

        [DisplayName("Away")]
        public int Fk_Away { get; set; }


        [DisplayName(nameof(HomeScore))]
        public int HomeScore { get; set; }

        [DisplayName(nameof(AwayScore))]
        public int AwayScore { get; set; }

        [DisplayName(nameof(StartTime))]
        public DateTime StartTime { get; set; }

        [DisplayName(nameof(IsEnded))]
        public bool IsEnded { get; set; }

        [DisplayName(nameof(_365_MatchId))]
        public string _365_MatchId { get; set; }
    }
}
