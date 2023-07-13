using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogicMicroservices.Areas.TeamDataArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Team")]
    [ApiExplorerSettings(GroupName = "Team")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class FormationPositionController : ExtendControllerBase
    {
        public FormationPositionController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }

        [HttpPost]
        [Route(nameof(UpdateFormationPositions))]
        public IActionResult UpdateFormationPositions([FromQuery] _365CompetitionsEnum _365CompetitionsEnum)
        {
            _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.FormationPositionDataHelper.RunUpdateFormationPositions(_365CompetitionsEnum));

            return Ok();
        }
    }
}
