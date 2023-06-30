using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogicMicroservices.Areas.StandingsDataArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Standings")]
    [ApiExplorerSettings(GroupName = "Standings")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class StandingsController : ExtendControllerBase
    {
        public StandingsController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }

        [HttpPost]
        [Route(nameof(UpdateStandings))]
        public IActionResult UpdateStandings([FromQuery] _365CompetitionsEnum _365CompetitionsEnum)
        {
            _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.StandingsDataHelper.RunUpdateStandings(_365CompetitionsEnum));

            return Ok();
        }
    }
}
