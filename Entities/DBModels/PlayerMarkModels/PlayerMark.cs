using Entities.DBModels.TeamModels;

namespace Entities.DBModels.PlayerMarkModels
{
    public class PlayerMark : AuditEntity
    {
        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public Player Player { get; set; }

        [DisplayName(nameof(Mark))]
        [ForeignKey(nameof(Mark))]
        public int Fk_Mark { get; set; }

        [DisplayName(nameof(Mark))]
        public Mark Mark { get; set; }

        [DisplayName(nameof(Value))]
        public string Value { get; set; }

        [DisplayName(nameof(Count))]
        public int? Count { get; set; }

        [DisplayName(nameof(Used))]
        public int? Used { get; set; }

        [DisplayName(nameof(PlayerMarkGameWeaks))]
        public List<PlayerMarkGameWeak> PlayerMarkGameWeaks { get; set; }

        [DisplayName(nameof(PlayerMarkTeamGameWeaks))]
        public List<PlayerMarkTeamGameWeak> PlayerMarkTeamGameWeaks { get; set; }

        [DisplayName(nameof(PlayerMarkGameWeakScores))]
        public List<PlayerMarkGameWeakScore> PlayerMarkGameWeakScores { get; set; }
    }
}
