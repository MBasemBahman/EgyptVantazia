using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using FantasyLogic;
using FantasyLogicMicroservices.Controllers;

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
        public IActionResult UpdatePlayers([FromQuery] TeamParameters parameters)
        {
            _fantasyUnitOfWork.PlayerDataHelper.RunUpdatePlayers(parameters);

            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdatePlayersStates))]
        public IActionResult UpdatePlayersStates(GameWeakParameters parameters)
        {
            _fantasyUnitOfWork.PlayerStateCalc.RunPlayersStateCalculations(parameters);

            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdatePlayersStatePositions))]
        public IActionResult UpdatePlayersStatePositions(GameWeakParameters parameters)
        {
            _fantasyUnitOfWork.PlayerStateCalc.RunPlayersStatePositions(parameters);

            return Ok();
        }
    }
}
