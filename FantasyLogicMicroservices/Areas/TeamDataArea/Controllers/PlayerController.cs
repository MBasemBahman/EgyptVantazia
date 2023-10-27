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
        public IActionResult UpdatePlayers(
            [FromQuery] _365CompetitionsEnum _365CompetitionsEnum,
            [FromQuery] bool inDebug)
        {
            if (inDebug)
            {
                _fantasyUnitOfWork.PlayerDataHelper.RunUpdatePlayers(_365CompetitionsEnum);
            }
            else
            {
                _ = BackgroundJob.Enqueue( () => _fantasyUnitOfWork.PlayerDataHelper.RunUpdatePlayers(_365CompetitionsEnum));
            }

            return Ok();
        }
    }
}
