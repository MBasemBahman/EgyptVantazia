using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;

namespace Entities.DBModels.PlayerStateModels
{
    public class PlayerSeasonScoreState : AuditEntity
    {
        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public Player Player { get; set; }

        [DisplayName(nameof(ScoreState))]
        [ForeignKey(nameof(ScoreState))]
        public int Fk_ScoreState { get; set; }

        [DisplayName(nameof(ScoreState))]
        public ScoreState ScoreState { get; set; }

        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Season))]
        public Season Season { get; set; }

        [DisplayName(nameof(Points))]
        public double Points { get; set; }

        [DisplayName(nameof(Value))]
        public double Value { get; set; }

        [DisplayName(nameof(Percent))]
        public double Percent { get; set; }
    }
}
