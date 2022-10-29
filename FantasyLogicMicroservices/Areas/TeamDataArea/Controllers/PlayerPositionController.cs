using FantasyLogic;
using FantasyLogicMicroservices.Controllers;

namespace FantasyLogicMicroservices.Areas.TeamDataArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Team")]
    [ApiExplorerSettings(GroupName = "Team")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class PlayerPositionController : ExtendControllerBase
    {
        public PlayerPositionController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }

        [HttpPost]
        [Route(nameof(UpdatePositions))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult UpdatePositions()
        {
            _fantasyUnitOfWork.PlayerPositionDataHelper.RunUpdatePositions(1);

            return Ok();
        }
    }
}
