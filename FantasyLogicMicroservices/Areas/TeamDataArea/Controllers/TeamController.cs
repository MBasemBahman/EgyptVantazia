using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;
using static Contracts.EnumData.DBModelsEnum;
using static Contracts.EnumData.HanfireEnum;

namespace FantasyLogicMicroservices.Areas.TeamDataArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Team")]
    [ApiExplorerSettings(GroupName = "Team")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class TeamController : ExtendControllerBase
    {
        public TeamController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }

        [HttpPost]
        [Route(nameof(UpdateTeams))]
        public IActionResult UpdateTeams([FromQuery] _365CompetitionsEnum _365CompetitionsEnum)
        {
            _ = BackgroundJob.Enqueue(HanfireQueuesEnum.DailyTasks.ToString(), () => _fantasyUnitOfWork.TeamDataHelper.RunUpdateTeams(_365CompetitionsEnum));

            return Ok();
        }
    }
}
