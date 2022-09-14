using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.StandingsModels
{
    public class StandingsParameters:RequestParameters
    {
        public int Fk_Season { get; set; }

        public int Fk_Team { get; set; }
    }
    public class StandingsModel : AuditEntity
    {
        public int Fk_Season { get; set; }

        public int Fk_Team { get; set; }

        [DisplayName(nameof(GamePlayed))]
        public int GamePlayed { get; set; }

        [DisplayName(nameof(GamesWon))]
        public int GamesWon { get; set; }

        [DisplayName(nameof(GamesLost))]
        public int GamesLost { get; set; }

        [DisplayName(nameof(GamesEven))]
        public int GamesEven { get; set; }

        [DisplayName(nameof(_365_For))]
        public int _365_For { get; set; }

        [DisplayName(nameof(Against))]
        public int Against { get; set; }

        [DisplayName(nameof(Ratio))]
        public int Ratio { get; set; }

        [DisplayName(nameof(Points))]
        public int Points { get; set; }
    }
}
