﻿using Entities.DBModels.TeamModels;

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

        [DisplayName(nameof(Count))]
        public int? Count { get; set; }

        [DisplayName(nameof(Used))]
        public int? Used { get; set; }

        public DateTime? DateTo { get; set; }

        [DisplayName(nameof(Percent))]
        public int Percent { get; set; }

        [DisplayName($"{nameof(Notes)}{PropertyAttributeConstants.ArLang}")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [DisplayName(nameof(PlayerMarkTeamGameWeaks))]
        public List<PlayerMarkTeamGameWeak> PlayerMarkTeamGameWeaks { get; set; }

        [DisplayName(nameof(PlayerMarkReasonMatches))]
        public List<PlayerMarkReasonMatch> PlayerMarkReasonMatches { get; set; }

        [DisplayName(nameof(PlayerMarkGameWeakScores))]
        public List<PlayerMarkGameWeakScore> PlayerMarkGameWeakScores { get; set; }

        [DisplayName(nameof(PlayerMarkLang))]
        public PlayerMarkLang PlayerMarkLang { get; set; }
    }
    public class PlayerMarkLang : LangEntity<PlayerMark>
    {
        [DisplayName($"{nameof(Notes)}{PropertyAttributeConstants.EnLang}")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
    }
}
