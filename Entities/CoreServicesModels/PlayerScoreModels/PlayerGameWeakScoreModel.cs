﻿using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PlayerScoreModels
{
    public class PlayerGameWeakScoreParameters : RequestParameters
    {
        [DisplayName(nameof(IsCanNotEdit))]
        public bool? IsCanNotEdit { get; set; }

        [DisplayName(nameof(PlayerGameWeak))]
        public int Fk_PlayerGameWeak { get; set; }

        [DisplayName(nameof(PlayerGameWeak))]
        public int Fk_TeamGameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(Player))]
        public int Fk_Player { get; set; }

        [DisplayName(nameof(Player))]
        public int Fk_TeamIgnored { get; set; }

        public int Fk_ScoreType { get; set; }

        public List<int> Fk_ScoreTypes { get; set; }

        public int? FinalValueFrom { get; set; }

        public int? FinalValueTo { get; set; }

        public int? PointsFrom { get; set; }

        public int? PointsTo { get; set; }

        public bool CheckCleanSheet { get; set; }

        public bool CheckReceiveGoals { get; set; }

        public List<int> Fk_Players { get; set; }
        public List<int> Fk_Teams { get; set; }
        public int Fk_Season { get; set; }
        public bool? IsEnded { get; set; }
        public double RateFrom { get; set; }
        public double RateTo { get; set; }

        public string DashboardSearch { get; set; }

        public bool CheckHaveValue { get; set; }
    }

    public class PlayerGameWeakScoreModel : AuditEntity
    {
        [DisplayName(nameof(PlayerGameWeak))]
        public int Fk_PlayerGameWeak { get; set; }
        public PlayerGameWeakModel PlayerGameWeak { get; set; }

        public int Fk_Team { get; set; }

        [DisplayName(nameof(ScoreType))]
        public int Fk_ScoreType { get; set; }

        [DisplayName(nameof(ScoreType))]
        public ScoreTypeModel ScoreType { get; set; }

        [DisplayName(nameof(Value))]
        public string Value { get; set; }

        [DisplayName(nameof(FinalValue))]
        public int FinalValue { get; set; }

        [DisplayName(nameof(GameTime))]
        public double GameTime { get; set; }

        [DisplayName(nameof(Points))]
        public int Points { get; set; }

        [DisplayName(nameof(IsOut))]
        public bool? IsOut { get; set; }

        [DisplayName(nameof(IsCanNotEdit))]
        public bool IsCanNotEdit { get; set; }
    }

    public class PlayerGameWeakScoreCreateOrEditModel
    {

        [DisplayName(nameof(ScoreType))]
        public int Fk_ScoreType { get; set; }

        [DisplayName(nameof(Value))]
        public string Value { get; set; }

        [DisplayName(nameof(FinalValue))]
        public int FinalValue { get; set; }

        [DisplayName(nameof(Points))]
        public int Points { get; set; }

        [DisplayName(nameof(GameTime))]
        public double GameTime { get; set; }

        public int Id { get; set; }

        [DisplayName(nameof(IsCanNotEdit))]
        public bool IsCanNotEdit { get; set; }
    }

    public class PlayerTotalScoreModel
    {
        [DisplayName(nameof(Points))]
        public int Points { get; set; }

        [DisplayName(nameof(FinalValue))]
        public int FinalValue { get; set; }
    }
}
