﻿using API.Areas.TeamArea.Models;
using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.TeamModels;
using System.ComponentModel;

namespace API.Areas.PlayerScoreArea.Models
{
    public class ScoreTypeDto : ScoreTypeModel
    {
        [SwaggerIgnore]
        [DisplayName(nameof(_365_TypeId))]
        public new string _365_TypeId { get; set; }

        public new PlayerDto BestPlayer { get; set; }
    }
}
