using Entities.DBModels.PlayerScoreModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.PlayerScoreModels
{
    public class PlayerGameWeakScoreParameters:RequestParameters
    {
        public int Fk_PlayerGameWeak { get; set; }

        public int Fk_ScoreType { get; set; }
    }
    public class PlayerGameWeakScoreModel : AuditEntity
    {
        public int Fk_PlayerGameWeak { get; set; }

        public int Fk_ScoreType { get; set; }

        [DisplayName(nameof(Value))]
        public int Value { get; set; }

        [DisplayName(nameof(Points))]
        public int Points { get; set; }

    }
}
