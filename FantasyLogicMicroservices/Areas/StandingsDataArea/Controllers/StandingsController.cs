using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;
using static Contracts.EnumData.DBModelsEnum;
using static Contracts.EnumData.HanfireEnum;

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
            if (_365CompetitionsEnum == 0)
            {
                _ = BackgroundJob.Enqueue( () => _fantasyUnitOfWork.StandingsDataHelper.RunUpdateStandings(_365CompetitionsEnum.Egypt));
                _ = BackgroundJob.Enqueue( () => _fantasyUnitOfWork.StandingsDataHelper.RunUpdateStandings(_365CompetitionsEnum.KSA));
                _ = BackgroundJob.Enqueue( () => _fantasyUnitOfWork.StandingsDataHelper.RunUpdateStandings(_365CompetitionsEnum.EPL));
            }
            else
            {
                _ = BackgroundJob.Enqueue( () => _fantasyUnitOfWork.StandingsDataHelper.RunUpdateStandings(_365CompetitionsEnum));
            }

            return Ok();
        }
    }
}
