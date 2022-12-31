using Entities.CoreServicesModels.SeasonModels;
using FantasyLogic;
using FantasyLogicMicroservices.Controllers;

namespace FantasyLogicMicroservices.Areas.GamesArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Games")]
    [ApiExplorerSettings(GroupName = "Games")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class GameResultController : ExtendControllerBase
    {
        public GameResultController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }

        [HttpPost]
        [Route(nameof(UpdateGameResult))]
        public IActionResult UpdateGameResult(
            [FromQuery] TeamGameWeakParameters parameters,
            [FromQuery] bool runBonus,
            [FromQuery] bool inDebug,
            [FromQuery] bool runAll)
        {
            _fantasyUnitOfWork.GameResultDataHelper.RunUpdateGameResult(parameters, runBonus, inDebug, runAll);

            return Ok();
        }
    }
}
