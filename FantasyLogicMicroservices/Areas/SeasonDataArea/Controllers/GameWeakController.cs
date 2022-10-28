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
        public IActionResult UpdateGameWeak()
        {
            _fantasyUnitOfWork.GameWeakDataHelper.RunUpdateGameWeaks(1);

            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdateCurrentGameWeak))]
        public IActionResult UpdateCurrentGameWeak()
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            _ = BackgroundJob.Schedule(() => _fantasyUnitOfWork.GameWeakDataHelper.UpdateCurrentGameWeak(season.Id, season._365_SeasonId.ParseToInt()), TimeSpan.FromMinutes(1));

            return Ok();
        }
    }
}
