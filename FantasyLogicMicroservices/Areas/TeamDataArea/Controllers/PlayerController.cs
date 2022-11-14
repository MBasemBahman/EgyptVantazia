using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using FantasyLogic;
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
        public IActionResult UpdatePlayersStates([FromQuery] GameWeakParameters gameWeakParameters, [FromQuery] PlayerParameters playerParameters)
        {
            _fantasyUnitOfWork.PlayerStateCalc.RunPlayersStateCalculations(gameWeakParameters, playerParameters);

            return Ok();
        }
    }
}
