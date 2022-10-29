using API.Areas.TeamArea.Models;
using Entities.CoreServicesModels.PlayerStateModels;

namespace API.Areas.PlayerStateArea.Models
{
    public class ScoreStateDto : ScoreStateModel
    {
        public new PlayerDto BestPlayer { get; set; }
    }
}
