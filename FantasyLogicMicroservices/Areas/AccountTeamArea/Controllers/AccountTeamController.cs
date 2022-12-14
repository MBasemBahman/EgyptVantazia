using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;

namespace FantasyLogicMicroservices.Areas.AccountTeamArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("AccountTeam")]
    [ApiExplorerSettings(GroupName = "AccountTeam")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class AccountTeamController : ExtendControllerBase
    {
        public AccountTeamController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }

        [HttpPost]
        [Route(nameof(UpdateAccountTeamsPoints))]
        public IActionResult UpdateAccountTeamsPoints(
            [FromQuery] int fk_GameWeak,
            [FromQuery] int fk_AccountTeam,
            [FromQuery] bool inDebug)
        {
            if (inDebug)
            {
                _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(fk_GameWeak, fk_AccountTeam, true);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(fk_GameWeak, fk_AccountTeam, false));
            }

            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdatePrivateLeaguesRanking))]
        public IActionResult UpdatePrivateLeaguesRanking()
        {
            //_fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(fk_GameWeak);

            _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.PrivateLeagueClac.RunPrivateLeaguesRanking());

            return Ok();
        }
    }
}
