using Entities.CoreServicesModels.SeasonModels;
using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using static Contracts.EnumData.DBModelsEnum;

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
            [FromQuery] _365CompetitionsEnum _365CompetitionsEnum,
            [FromQuery] TeamGameWeakParameters parameters,
            [FromQuery] bool runBonus,
            [FromQuery] bool inDebug,
            [FromQuery] bool runAll,
            [FromQuery] bool stopAll,
            [FromQuery] bool statisticsOnly)
        {
            _fantasyUnitOfWork.GameResultDataHelper.RunUpdateGameResult(_365CompetitionsEnum, parameters, runBonus, inDebug, runAll, stopAll, statisticsOnly);

            return Ok();
        }
    }
}
