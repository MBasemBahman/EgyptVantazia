﻿using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;

namespace FantasyLogicMicroservices.Areas.TeamDataArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Team")]
    [ApiExplorerSettings(GroupName = "Team")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class PlayerController : ExtendControllerBase
    {
        public PlayerController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }

        [HttpPost]
        [Route(nameof(UpdatePlayers))]
        public IActionResult UpdatePlayers()
        {
            _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.PlayerDataHelper.RunUpdatePlayers());

            RecurringJob.AddOrUpdate("UpdatePlayers", () => _fantasyUnitOfWork.PlayerDataHelper.RunUpdatePlayers(), "0 2 * * *", TimeZoneInfo.Utc);

            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdatePlayersStates))]
        public IActionResult UpdatePlayersStates([FromQuery] int fk_GameWeak, [FromQuery] string _365_MatchId)
        {
            //_fantasyUnitOfWork.GamesDataHelper.TransferAccountTeamPlayers(50, 55, 2);
            //_fantasyUnitOfWork.PlayerStateCalc.UpdateTop15(45, 0).Wait();

            _fantasyUnitOfWork.PlayerStateCalc.RunPlayersStateCalculations(fk_GameWeak, _365_MatchId);

            return Ok();
        }
    }
}
