using Entities.CoreServicesModels.SeasonModels;
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
            [FromQuery] List<int> fk_Players,
            [FromQuery] bool inDebug)
        {
            if (inDebug)
            {
                _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(fk_GameWeak, fk_AccountTeam, fk_Players, inDebug);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(fk_GameWeak, fk_AccountTeam, fk_Players, inDebug));
            }

            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdateAccountTeamGameWeakRanking))]
        public IActionResult UpdateAccountTeamGameWeakRanking(
            [FromQuery] int fk_GameWeak,
            [FromQuery] bool inDebug)
        {
            GameWeakModel gameWeek = _unitOfWork.Season.GetGameWeakbyId(fk_GameWeak, otherLang: false);

            if (inDebug)
            {
                _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamGameWeakRanking(gameWeek, gameWeek.Fk_Season);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamGameWeakRanking(gameWeek, gameWeek.Fk_Season));
            }
            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdatePrivateLeaguesRanking))]
        public IActionResult UpdatePrivateLeaguesRanking([FromQuery] bool indebug)
        {
            if (indebug)
            {
                _fantasyUnitOfWork.PrivateLeagueClac.RunPrivateLeaguesRanking(indebug);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.PrivateLeagueClac.RunPrivateLeaguesRanking(indebug));
            }

            return Ok();
        }
    }
}
