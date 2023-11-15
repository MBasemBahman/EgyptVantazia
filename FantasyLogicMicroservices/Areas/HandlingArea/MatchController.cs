using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;
using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;
using System.Diagnostics;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogicMicroservices.Areas.HandlingArea
{
    [ApiVersion("1.0")]
    [Area("Handling")]
    [ApiExplorerSettings(GroupName = "Handling")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class MatchController : ExtendControllerBase
    {
        public MatchController(
        ILoggerManager logger,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        FantasyUnitOfWork fantasyUnitOfWork,
        IOptions<AppSettings> appSettings) : base(logger, unitOfWork, linkGenerator, environment, fantasyUnitOfWork, appSettings)
        {
        }


        [HttpPost]
        [Route(nameof(ResetMatch))]
        public IActionResult ResetMatch(
            [FromQuery] _365CompetitionsEnum _365CompetitionsEnum,
            [FromQuery] int fk_TeamGameWeak,
            [FromQuery] int fk_GameWeak,
            [FromQuery] List<int> fk_Teams,
            [FromQuery] bool ignoreResetPlayer,
            [FromQuery] bool ignoreScoreState,
            [FromQuery] bool ignoreCalculations)
        {
            if (fk_TeamGameWeak > 0)
            {
                TeamGameWeakModel match = _unitOfWork.Season.GetTeamGameWeakbyId(fk_TeamGameWeak, otherLang: false);

                if (!ignoreResetPlayer)
                {
                    _unitOfWork.PlayerScore.ResetPlayerGameWeak(fk_TeamGameWeak, 0, 0, 0);
                }

                if (!ignoreScoreState)
                {
                    _unitOfWork.PlayerState.ResetPlayerGameWeakScoreState(0, match.Fk_GameWeak, match.Fk_Home);
                    _unitOfWork.PlayerState.ResetPlayerGameWeakScoreState(0, match.Fk_GameWeak, match.Fk_Away);
                }

                _unitOfWork.Save().Wait();

                if (!ignoreCalculations)
                {
                    fk_Teams = new()
                    {
                        match.Fk_Home,
                        match.Fk_Away
                    };

                    _fantasyUnitOfWork.PlayerStateCalc.RunPlayersStateCalculations(_365CompetitionsEnum, 0, null, null, fk_Teams, true, false);

                    _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(_365CompetitionsEnum, match.Fk_GameWeak, 0, null, fk_Teams, false);
                }
            }
            else if (fk_GameWeak > 0 &&
                     fk_Teams.Any())
            {

                foreach (int fk_Team in fk_Teams)
                {
                    if (!ignoreResetPlayer)
                    {
                        _unitOfWork.PlayerScore.ResetPlayerGameWeak(0, 0, fk_GameWeak, fk_Team);
                    }
                    if (!ignoreScoreState)
                    {
                        _unitOfWork.PlayerState.ResetPlayerGameWeakScoreState(0, fk_GameWeak, fk_Team);
                    }
                }

                _unitOfWork.Save().Wait();

                if (!ignoreCalculations)
                {
                    _fantasyUnitOfWork.PlayerStateCalc.RunPlayersStateCalculations(_365CompetitionsEnum, 0, null, null, fk_Teams, true, false);

                    _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(_365CompetitionsEnum, fk_GameWeak, 0, null, fk_Teams, false);
                }

            }

            return Ok();
        }

        [HttpPost]
        [Route(nameof(ResetAccountTeam))]
        public async Task<IActionResult> ResetAccountTeam(
            [FromQuery] int fk_GameWeak,
            [FromQuery] int fk_AccounTeam,
            [FromQuery] bool resetTeam,
            [FromQuery] bool removeOldTeam,
            [FromQuery] bool inDebug)
        {
            if (inDebug)
            {
                await ResetAccountTeamTask(fk_GameWeak, fk_AccounTeam, resetTeam, removeOldTeam, inDebug);
            }
            else
            {
                _ = BackgroundJob.Enqueue(() => ResetAccountTeamTask(fk_GameWeak, fk_AccounTeam, resetTeam, removeOldTeam, inDebug));
            }

            return Ok();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task ResetAccountTeamTask(int fk_GameWeak,
            int fk_AccounTeam,
            bool resetTeam,
            bool removeOldTeam,
            bool inDebug)
        {
            var teams = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new Entities.CoreServicesModels.AccountTeamModels.AccountTeamGameWeakParameters
            {
                Fk_GameWeak = fk_GameWeak,
                Fk_AccountTeam = fk_AccounTeam,
            }, otherLang: false).Select(a => new
            {
                a.Fk_AccountTeam,
                a.Fk_GameWeak,
                a.Id,
            }).ToList();

            int count = 0;

            foreach (var team in teams)
            {
                fk_GameWeak = team.Fk_GameWeak;
                fk_AccounTeam = team.Fk_AccountTeam;

                GameWeak gameWeak = await _unitOfWork.Season.FindGameWeakbyId(fk_GameWeak, trackChanges: false);
                GameWeak nextGameWeak = null;

                do
                {
                    gameWeak = await _unitOfWork.Season.FindGameWeakbyId(fk_GameWeak, trackChanges: false);
                    nextGameWeak = await _unitOfWork.Season.FindGameWeakby365Id((gameWeak._365_GameWeakId.ParseToInt() + 1).ToString(), gameWeak.Fk_Season, trackChanges: false);

                    bool haveTeam = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new Entities.CoreServicesModels.AccountTeamModels.AccountTeamGameWeakParameters
                    {
                        Fk_GameWeak = nextGameWeak.Id,
                        Fk_AccountTeam = fk_AccounTeam,
                    }, otherLang: false).Any();

                    if ((haveTeam == false || removeOldTeam) && gameWeak.IsNext == false)
                    {
                        count++;
                        _fantasyUnitOfWork.GamesDataHelper.TransferAccountTeamPlayers(nextGameWeak.Id, gameWeak.Id, gameWeak._365_GameWeakId.ParseToInt(), nextGameWeak.Fk_Season, resetTeam, fk_AccounTeam, inDebug);
                    }

                    fk_GameWeak++;

                } while (gameWeak.IsNext == false);

            }
        }

		[HttpPost]
		[Route(nameof(RecurringJobCheck))]
		public IActionResult RecurringJobCheck()
		{
			RecurringJob.AddOrUpdate("RecurringJobCheck", () => RecurringJobCheckTask(), CronExpression.EveryMinutes(5), TimeZoneInfo.Local);

			return Ok();
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		public void RecurringJobCheckTask()
		{
			return;
		}
	}
}
