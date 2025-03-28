﻿using Dashboard.Areas.SeasonEntity.Models;
using Dashboard.Areas.TeamEntity.Models;
using Entities.CoreServicesModels.PlayerScoreModels;
using System.ComponentModel;

namespace Dashboard.Areas.PlayerScoreEntity.Models
{
    public class PlayerGameWeakFilter : DtParameters
    {
        public int Fk_GameWeak { get; set; }
        public int Fk_Season { get; set; }
        public bool? IsEnded { get; set; }
        public List<int> Fk_Players { get; set; }
        public List<int> Fk_Teams { get; set; }
        public int Fk_Player { get; set; }
        [DefaultValue(0)]
        public double RateFrom { get; set; }
        [DefaultValue(10)]
        public double RateTo { get; set; }

        public double PointsFrom { get; set; }
        public double PointsTo { get; set; }

        public int Fk_Home { get; set; }
        public int Fk_Away { get; set; }

        public string DashboardSearch { get; set; }
    }
    public class PlayerGameWeakDto : PlayerGameWeakModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public new string LastModifiedAt { get; set; }

        [DisplayName(nameof(TeamGameWeak))]
        public new TeamGameWeakDto TeamGameWeak { get; set; }

        [DisplayName(nameof(Player))]
        public new PlayerDto Player { get; set; }

        [DisplayName(nameof(PlayerGameWeakScores))]
        public new List<PlayerGameWeakScoreDto> PlayerGameWeakScores { get; set; }
    }

    public enum PlayerGameWeakProfileItems
    {
        Details = 1,
        PlayerGameWeakScore = 2
    }

    public enum PlayerGameWeakReturnPage
    {
        Index = 1,
        PlayerGameWeakProfile = 2,
        TeamProfile = 3
    }
}
