using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogicMicroservices.Areas.GamesArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Games")]
    [ApiExplorerSettings(GroupName = "Games")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class ScoreTypeController : ExtendControllerBase
    {
        public ScoreTypeController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }

        [HttpPost]
        [Route(nameof(UpdateScoreTypes))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult UpdateScoreTypes(_365CompetitionsEnum _365CompetitionsEnum, bool inDebug)
        {
            _fantasyUnitOfWork.ScoreTypeDataHelper.RunUpdateStates(_365CompetitionsEnum, 1, inDebug);

            return Ok();
        }
    }
}
