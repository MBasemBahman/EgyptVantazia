﻿using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerStateModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PlayerStateModels
{
    public class PlayerSeasonScoreStateParameters : RequestParameters
    {
        public int Fk_Player { get; set; }
        public List<int> Fk_Players { get; set; }
        public int Fk_ScoreState { get; set; }
        public List<int> Fk_ScoreStates { get; set; }
        public int Fk_Season { get; set; }
        public List<int> Fk_Seasons { get; set; }

        public int Fk_PlayerPosition { get; set; }

        public double? PointsFrom { get; set; }
        public double? PointsTo { get; set; }
        public double? PercentFrom { get; set; }
        public double? PercentTo { get; set; }
        public bool? IsTop15 { get; set; }
        public string DashboardSearch { get; set; }

        public bool GetMonthPlayer { get; set; }

        public int From_365_GameWeakIdValue { get; set; }
        public int To_365_GameWeakIdValue { get; set; }
    }

    public class MontlyGameWeakFromToModel
    {
        public int From_365_GameWeakIdValue { get; set; }
        public int To_365_GameWeakIdValue { get; set; }
    }

    public class PlayerSeasonScoreStateModel : AuditEntity
    {
        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public PlayerModel Player { get; set; }

        [DisplayName(nameof(ScoreState))]
        [ForeignKey(nameof(ScoreState))]
        public int Fk_ScoreState { get; set; }

        [DisplayName(nameof(ScoreState))]
        public ScoreStateModel ScoreState { get; set; }

        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Season))]
        public SeasonModel Season { get; set; }

        [DisplayName(nameof(Points))]
        public double Points { get; set; }

        [DisplayName(nameof(Value))]
        public double Value { get; set; }

        [DisplayName(nameof(Percent))]
        public double Percent { get; set; }

        [DisplayName(nameof(Top15))]
        public int? Top15 { get; set; }
    }

    public class PlayerSeasonScoreStateCreateOrEditModel
    {
        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(ScoreState))]
        [ForeignKey(nameof(ScoreState))]
        public int Fk_ScoreState { get; set; }

        [DisplayName(nameof(Points))]
        public double Points { get; set; }


        [DisplayName(nameof(Value))]
        public double Value { get; set; }

        [DisplayName(nameof(Percent))]
        public double Percent { get; set; }
    }

    public class PlayerSeasonScoreStateCalcModel
    {
        [DisplayName(nameof(Points))]
        public double Points { get; set; }

        [DisplayName(nameof(Value))]
        public double Value { get; set; }

        [DisplayName(nameof(Percent))]
        public double Percent { get; set; }
    }
}
