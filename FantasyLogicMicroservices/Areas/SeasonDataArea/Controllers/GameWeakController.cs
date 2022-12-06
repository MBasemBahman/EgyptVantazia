using Entities.CoreServicesModels.SeasonModels;
using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;
using IntegrationWith365;

namespace FantasyLogicMicroservices.Areas.SeasonDataArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Season")]
    [ApiExplorerSettings(GroupName = "Season")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class GameWeakController : ExtendControllerBase
    {
        private readonly _365Services _365Services;

        public GameWeakController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        _365Services _365Services,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
            this._365Services = _365Services;
        }

        [HttpPost]
        [Route(nameof(UpdateGameWeak))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult UpdateGameWeak()
        {
            _fantasyUnitOfWork.GameWeakDataHelper.RunUpdateGameWeaks();

            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdateCurrentGameWeak))]
        public IActionResult UpdateCurrentGameWeak(int fk_GameWeak)
        {
            _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.GamesDataHelper.UpdateCurrentGameWeak(fk_GameWeak));

            return Ok();
        }
    }
}
