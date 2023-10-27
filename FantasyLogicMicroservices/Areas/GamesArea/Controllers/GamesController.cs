using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogicMicroservices.Areas.GamesArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Games")]
    [ApiExplorerSettings(GroupName = "Games")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class GamesController : ExtendControllerBase
    {
        public GamesController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }

        [HttpPost]
        [Route(nameof(UpdateGames))]
        public IActionResult UpdateGames([FromQuery] _365CompetitionsEnum _365CompetitionsEnum)
        {
            if (_365CompetitionsEnum == 0)
            {
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.GamesDataHelper.RunUpdateGames(_365CompetitionsEnum.Egypt));
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.GamesDataHelper.RunUpdateGames(_365CompetitionsEnum.KSA));
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.GamesDataHelper.RunUpdateGames(_365CompetitionsEnum.EPL));
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.GamesDataHelper.RunUpdateGames(_365CompetitionsEnum));
            }

            return Ok();
        }
    }
}
