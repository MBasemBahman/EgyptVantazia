namespace Entities.DBModels.PlayerScoreModels
{
    public class PlayerGameWeakScore : AuditEntity
    {
        [DisplayName(nameof(PlayerGameWeak))]
        [ForeignKey(nameof(PlayerGameWeak))]
        public int Fk_PlayerGameWeak { get; set; }

        [DisplayName(nameof(PlayerGameWeak))]
        public PlayerGameWeak PlayerGameWeak { get; set; }

        [DisplayName(nameof(ScoreType))]
        [ForeignKey(nameof(ScoreType))]
        public int Fk_ScoreType { get; set; }

        [DisplayName(nameof(ScoreType))]
        public ScoreType ScoreType { get; set; }

        [DisplayName(nameof(Value))]
        public string Value { get; set; }

        [DisplayName(nameof(Points))]
        public int Points { get; set; }
    }
}
