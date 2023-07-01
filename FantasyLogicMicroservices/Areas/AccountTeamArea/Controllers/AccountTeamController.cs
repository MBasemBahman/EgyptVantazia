using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;
using static Contracts.EnumData.DBModelsEnum;

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
            [FromQuery] _365CompetitionsEnum _365CompetitionsEnum,
            [FromQuery] int fk_GameWeak,
            [FromQuery] int fk_AccountTeam,
            [FromQuery] List<int> fk_Players,
            [FromQuery] bool inDebug)
        {
            if (inDebug)
            {
                _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(_365CompetitionsEnum, fk_GameWeak, fk_AccountTeam, fk_Players, null, inDebug);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(_365CompetitionsEnum, fk_GameWeak, fk_AccountTeam, fk_Players, null, inDebug));
            }

            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdateAccountTeamGameWeakRanking))]
        public IActionResult UpdateAccountTeamGameWeakRanking(
            [FromQuery] int fk_GameWeak,
            [FromQuery] bool inDebug)
        {
            GameWeakModelForCalc gameWeek = _unitOfWork.Season.GetGameWeaksForCalc(new GameWeakParameters
            {
                Id = fk_GameWeak,
            }).FirstOrDefault();

            if (inDebug)
            {
                _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamGameWeakRanking(fk_GameWeak, gameWeek.Fk_Season);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamGameWeakRanking(fk_GameWeak, gameWeek.Fk_Season));
            }
            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdateAccountTeamRanking))]
        public IActionResult UpdateAccountTeamRanking(
           [FromQuery] _365CompetitionsEnum _365CompetitionsEnum,
           [FromQuery] int fk_GameWeak,
           [FromQuery] bool inDebug)
        {
            GameWeakModelForCalc gameWeek = _unitOfWork.Season.GetGameWeaksForCalc(new GameWeakParameters
            {
                Id = fk_GameWeak,
            }).FirstOrDefault();

            if (inDebug)
            {
                _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamRanking(_365CompetitionsEnum, gameWeek.Fk_Season);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamRanking(_365CompetitionsEnum, gameWeek.Fk_Season));
            }
            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdatePrivateLeaguesRanking))]
        public IActionResult UpdatePrivateLeaguesRanking(
            [FromQuery] _365CompetitionsEnum _365CompetitionsEnum,
            [FromQuery] bool indebug,
            [FromQuery] int? fk_GameWeak,
            [FromQuery] int id)
        {
            if (indebug)
            {
                _fantasyUnitOfWork.PrivateLeagueClac.RunPrivateLeaguesRanking(_365CompetitionsEnum, fk_GameWeak, id, indebug);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => _fantasyUnitOfWork.PrivateLeagueClac.RunPrivateLeaguesRanking(_365CompetitionsEnum, fk_GameWeak, id, indebug));
            }

            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdateAccountTeamUpdateCards))]
        public IActionResult UpdateAccountTeamUpdateCards(
            [FromQuery] bool indebug,
            [FromBody] AccountTeamsUpdateCards model)
        {
            if (indebug)
            {
                _unitOfWork.AccountTeam.UpdateAccountTeamUpdateCards(model);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => _unitOfWork.AccountTeam.UpdateAccountTeamUpdateCards(model));
            }

            return Ok();
        }
    }
}
