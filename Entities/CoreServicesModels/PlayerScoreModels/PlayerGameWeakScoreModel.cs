using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.PlayerScoreModels
{
    public class PlayerGameWeakScoreParameters : RequestParameters
    {
        [DisplayName(nameof(PlayerGameWeak))]
        public int Fk_PlayerGameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(Player))]
        public int Fk_Player { get; set; }

        public int Fk_ScoreType { get; set; }
    }

    public class PlayerGameWeakScoreModel : AuditEntity
    {
        [DisplayName(nameof(PlayerGameWeak))]
        public int Fk_PlayerGameWeak { get; set; }

        [DisplayName(nameof(ScoreType))]
        public int Fk_ScoreType { get; set; }

        [DisplayName(nameof(ScoreType))]
        public ScoreTypeModel ScoreType { get; set; }

        [DisplayName(nameof(Value))]
        public int Value { get; set; }

        [DisplayName(nameof(Points))]
        public int Points { get; set; }
    }

    public class PlayerGameWeakScoreCreateOrEditModel
    {

        [DisplayName(nameof(ScoreType))]
        [ForeignKey(nameof(ScoreType))]
        public int Fk_ScoreType { get; set; }

        [DisplayName(nameof(Value))]
        public int Value { get; set; }

        [DisplayName(nameof(Points))]
        public int Points { get; set; }

        public int Id { get; set; }
    }
}
