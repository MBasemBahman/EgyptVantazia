using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;

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
        public IActionResult UpdateTeams()
        {
            _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.TeamDataHelper.RunUpdateTeams());

            return Ok();
        }
    }
}
