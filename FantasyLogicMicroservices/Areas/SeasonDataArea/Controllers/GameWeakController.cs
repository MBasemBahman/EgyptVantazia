using FantasyLogic;
using FantasyLogicMicroservices.Controllers;

namespace FantasyLogicMicroservices.Areas.SeasonDataArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Season")]
    [ApiExplorerSettings(GroupName = "Season")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class GameWeakController : ExtendControllerBase
    {
        public GameWeakController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }

        [HttpGet]
        [Route(nameof(UpdateGameWeak))]
        public IActionResult UpdateGameWeak()
        {
            _fantasyUnitOfWork.GameWeakDataHelper.UpdateGameWeaks(1);

            return Ok();
        }
    }
}
