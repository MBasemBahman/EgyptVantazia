using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerStateModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PlayerStateModels
{
    public class PlayerGameWeakScoreStateParameters : RequestParameters
    {
        public int Fk_Player { get; set; }
        public List<int> Fk_Players { get; set; }
        public int Fk_ScoreState { get; set; }
        public List<int> Fk_ScoreStates { get; set; }
        public int? Fk_GameWeak { get; set; }
        public List<int> Fk_GameWeaks { get; set; }
        public double? PointsFrom { get; set; }
        public double? PointsTo { get; set; }
        public double? PercentFrom { get; set; }
        public double? PercentTo { get; set; }
        public double? ValueFrom { get; set; }
        public double? ValueTo { get; set; }
    }

    public class PlayerGameWeakScoreStateModel : AuditEntity
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

        [DisplayName(nameof(GameWeak))]
        [ForeignKey(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeakModel GameWeak { get; set; }

        [DisplayName(nameof(Points))]
        public double Points { get; set; }

        [DisplayName(nameof(PositionByPoints))]
        public int PositionByPoints { get; set; }

        [DisplayName(nameof(Value))]
        public double Value { get; set; }

        [DisplayName(nameof(PositionByValue))]
        public int PositionByValue { get; set; }

        [DisplayName(nameof(Percent))]
        public double Percent { get; set; }

        [DisplayName(nameof(PositionByPercent))]
        public int PositionByPercent { get; set; }
    }

    public class PlayerGameWeakScoreStateCreateOrEditModel
    {
        [DisplayName(nameof(Player))]
        [ForeignKey(nameof(Player))]
        public int Fk_Player { get; set; }
        
        [DisplayName(nameof(ScoreState))]
        [ForeignKey(nameof(ScoreState))]
        public int Fk_ScoreState { get; set; }
        
        [DisplayName(nameof(GameWeak))]
        [ForeignKey(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(Points))]
        public double Points { get; set; }

        [DisplayName(nameof(PositionByPoints))]
        public int PositionByPoints { get; set; }

        [DisplayName(nameof(Value))]
        public double Value { get; set; }

        [DisplayName(nameof(PositionByValue))]
        public int PositionByValue { get; set; }

        [DisplayName(nameof(Percent))]
        public double Percent { get; set; }

        [DisplayName(nameof(PositionByPercent))]
        public int PositionByPercent { get; set; }
    }

    public class PlayerGameWeakScoreStateCalcModel
    {
        [DisplayName(nameof(Points))]
        public double Points { get; set; }

        [DisplayName(nameof(PositionByPoints))]
        public int PositionByPoints { get; set; }

        [DisplayName(nameof(Value))]
        public double Value { get; set; }

        [DisplayName(nameof(PositionByValue))]
        public int PositionByValue { get; set; }

        [DisplayName(nameof(Percent))]
        public double Percent { get; set; }

        [DisplayName(nameof(PositionByPercent))]
        public int PositionByPercent { get; set; }
    }
}
