using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;

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
        public IActionResult UpdateGames()
        {
            //_fantasyUnitOfWork.GamesDataHelper.RunUpdateGames();

            _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.GamesDataHelper.RunUpdateGames());

            RecurringJob.AddOrUpdate("UpdateGames", () => _fantasyUnitOfWork.GamesDataHelper.RunUpdateGames(), "0 3 * * *", TimeZoneInfo.Utc);

            return Ok();
        }
    }
}
