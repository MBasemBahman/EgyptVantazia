﻿using Entities.CoreServicesModels.TeamModels;
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

        [DisplayName(nameof(Mark))]
        [ForeignKey(nameof(Mark))]
        public int Fk_Mark { get; set; }
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

        [DisplayName(nameof(Fk_TeamGameWeaks))]
        public List<int> Fk_TeamGameWeaks { get; set; }
        
        [DisplayName(nameof(Fk_PlayerMarkReasonMatches))]
        public List<int> Fk_PlayerMarkReasonMatches { get; set; }
    }
}
