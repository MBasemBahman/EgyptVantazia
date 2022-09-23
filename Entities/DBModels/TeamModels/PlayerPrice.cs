namespace Entities.DBModels.TeamModels
{
    public class PlayerPrice : AuditEntity
    {
        [DisplayName(nameof(Team))]
        [ForeignKey(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(Team))]
        public Team Team { get; set; }

        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public Player Player { get; set; }

        [DisplayName(nameof(BuyPrice))]
        public double BuyPrice { get; set; }

        [DisplayName(nameof(SellPrice))]
        public double SellPrice { get; set; }
    }
}
