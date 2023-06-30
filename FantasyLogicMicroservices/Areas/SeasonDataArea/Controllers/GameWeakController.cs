using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.TeamModels;
using FantasyLogic;
using FantasyLogicMicroservices.Controllers;
using Hangfire;
using IntegrationWith365;
using static Contracts.EnumData.DBModelsEnum;

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
        [Route(nameof(UpdateGameWeaks))]
        public IActionResult UpdateGameWeaks([FromQuery] _365CompetitionsEnum _365CompetitionsEnum, [FromQuery] bool inDebug)
        {
            _fantasyUnitOfWork.GameWeakDataHelper.UpdateSeasonGameWeaks(_365CompetitionsEnum, inDebug);

            return Ok();
        }


        [HttpPost]
        [Route(nameof(UpdateCurrentGameWeak))]
        public IActionResult UpdateCurrentGameWeak(_365CompetitionsEnum _365CompetitionsEnum, int id, bool resetTeam, int fk_AccounTeam, bool inDebug, bool skipReset)
        {
            _fantasyUnitOfWork.GamesDataHelper.UpdateCurrentGameWeak(_365CompetitionsEnum, id, resetTeam, fk_AccounTeam, inDebug, skipReset).Wait();

            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdateRecurringJob))]
        public IActionResult UpdateRecurringJob()
        {
            WeeklyRecurringJob();
            DailyRecurringJob();

            return Ok();
        }

        [HttpPost]
        [Route(nameof(RunRemoveOldRecurringJob))]
        public IActionResult RunRemoveOldRecurringJob()
        {
            _ = BackgroundJob.Enqueue(() => RemoveOldRecurringJob());

            return Ok();
        }

        private void DailyRecurringJob()
        {
            //// At 01:00 AM
            RecurringJob.AddOrUpdate("RunAccountTeamsCalculations", () => _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(_365CompetitionsEnum.Egypt, 0, 0, null, null, false), "0 */8 * * *");
            RecurringJob.AddOrUpdate("RunAccountTeamsCalculations", () => _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(_365CompetitionsEnum.KSA, 0, 0, null, null, false), "0 */9 * * *");

            // At 04:00 AM
            RecurringJob.AddOrUpdate("UpdatePrivateLeaguesRanking", () => _fantasyUnitOfWork.PrivateLeagueClac.RunPrivateLeaguesRanking(_365CompetitionsEnum.Egypt, null, 0, false), "0 */14 * * *");
            RecurringJob.AddOrUpdate("UpdatePrivateLeaguesRanking", () => _fantasyUnitOfWork.PrivateLeagueClac.RunPrivateLeaguesRanking(_365CompetitionsEnum.KSA, null, 0, false), "0 */15 * * *");

            // At 05:00 AM
            // Egypt
            RecurringJob.AddOrUpdate("UpdateGames", () => _fantasyUnitOfWork.GamesDataHelper.RunUpdateGames(_365CompetitionsEnum.Egypt), "0 */6 * * *");
            // KSA
            RecurringJob.AddOrUpdate("UpdateGames", () => _fantasyUnitOfWork.GamesDataHelper.RunUpdateGames(_365CompetitionsEnum.KSA), "0 */7 * * *");

            // At 06:00 AM
            // Egypt
            RecurringJob.AddOrUpdate("UpdateStandings", () => _fantasyUnitOfWork.StandingsDataHelper.RunUpdateStandings(_365CompetitionsEnum.Egypt), "0 */9 * * *");
            // KSA
            RecurringJob.AddOrUpdate("UpdateStandings", () => _fantasyUnitOfWork.StandingsDataHelper.RunUpdateStandings(_365CompetitionsEnum.KSA), "0 */10 * * *");

        }

        private void WeeklyRecurringJob()
        {
            // At 07:00 AM, only on Friday
            RecurringJob.AddOrUpdate("UpdateTeams", () => _fantasyUnitOfWork.TeamDataHelper.RunUpdateTeams(_365CompetitionsEnum.Egypt), "0 7 * * 5");
            RecurringJob.AddOrUpdate("UpdateTeams", () => _fantasyUnitOfWork.TeamDataHelper.RunUpdateTeams(_365CompetitionsEnum.KSA), "0 8 * * 5");

            // At 08:00 AM, only on Friday
            RecurringJob.AddOrUpdate("UpdatePlayers", () => _fantasyUnitOfWork.PlayerDataHelper.RunUpdatePlayers(_365CompetitionsEnum.Egypt), "0 8 * * 5");
            RecurringJob.AddOrUpdate("UpdatePlayers", () => _fantasyUnitOfWork.PlayerDataHelper.RunUpdatePlayers(_365CompetitionsEnum.KSA), "0 9 * * 5");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void RemoveOldRecurringJob()
        {
            _ = BackgroundJob.Enqueue(() => RemoveAccountTeamRecurringJob());

            // Egypt
            _ = BackgroundJob.Enqueue(() => RemovePlayersRecurringJob(_365CompetitionsEnum.Egypt));
            // KSA
            _ = BackgroundJob.Enqueue(() => RemovePlayersRecurringJob(_365CompetitionsEnum.KSA));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void RemoveAccountTeamRecurringJob()
        {
            List<int> accountTeams = _unitOfWork.AccountTeam.GetAccountTeams(new AccountTeamParameters()).Select(a => a.Id).ToList();
            foreach (int accountTeam in accountTeams)
            {
                RecurringJob.RemoveIfExists($"AccountTeamCalc-{accountTeam}");
            }

            List<int> accountTeamGameWeaks = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters()).Select(a => a.Id).ToList();
            foreach (int accountTeamGameWeak in accountTeamGameWeaks)
            {
                RecurringJob.RemoveIfExists($"AccountTeamGameWeakCalc-{accountTeamGameWeak}");
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void RemovePlayersRecurringJob([FromQuery] _365CompetitionsEnum _365CompetitionsEnum)
        {
            int gameWeek = _unitOfWork.Season.GetCurrentGameWeakId(_365CompetitionsEnum);

            List<int> players = _unitOfWork.Team.GetPlayers(new PlayerParameters
            {
                Fk_GameWeak = gameWeek
            }).Select(a => a.Id).ToList();

            foreach (int player in players)
            {
                RecurringJob.RemoveIfExists($"PlayerGameWeekStatesCalc-{gameWeek}-{player}");
            }
        }
    }
}
