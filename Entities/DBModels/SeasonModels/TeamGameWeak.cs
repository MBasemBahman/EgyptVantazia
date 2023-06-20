using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.TeamModels;

namespace Entities.DBModels.SeasonModels
{
    public class TeamGameWeak : AuditEntity
    {
        [DisplayName(nameof(Home))]
        [ForeignKey(nameof(Home))]
        public int Fk_Home { get; set; }

        [DisplayName(nameof(Home))]
        public Team Home { get; set; }

        [DisplayName(nameof(Away))]
        [ForeignKey(nameof(Away))]
        public int Fk_Away { get; set; }

        [DisplayName(nameof(Away))]
        public Team Away { get; set; }

        [DisplayName(nameof(GameWeak))]
        [ForeignKey(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeak GameWeak { get; set; }

        [DisplayName(nameof(HomeScore))]
        public int HomeScore { get; set; }

        [DisplayName(nameof(AwayScore))]
        public int AwayScore { get; set; }

        [DisplayName(nameof(StartTime))]
        public DateTime StartTime { get; set; }

        [DisplayName(nameof(IsEnded))]
        public bool IsEnded { get; set; }

        [DisplayName(nameof(IsDelayed))]
        [DefaultValue(false)]
        public bool IsDelayed { get; set; }

        [DisplayName(nameof(_365_MatchId))]
        public string _365_MatchId { get; set; }

        [DisplayName(nameof(LastUpdateId))]
        public string LastUpdateId { get; set; }

        [DisplayName(nameof(JobId))]
        public string JobId { get; set; }

        [DisplayName(nameof(SecondJobId))]
        public string SecondJobId { get; set; }

        [DisplayName(nameof(ThirdJobId))]
        public string ThirdJobId { get; set; }

        [DisplayName(nameof(IsCanNotEdit))]
        public bool IsCanNotEdit { get; set; }

        [DisplayName(nameof(IsActive))]
        public bool IsActive { get; set; }

        [DisplayName(nameof(PlayerGameWeaks))]
        public IList<PlayerGameWeak> PlayerGameWeaks { get; set; }
    }
}
