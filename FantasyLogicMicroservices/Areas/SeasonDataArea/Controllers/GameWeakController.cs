﻿using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.TeamModels;
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
        [Route(nameof(UpdateCurrentGameWeak))]
        public IActionResult UpdateCurrentGameWeak(int id, bool resetTeam, int fk_AccounTeam, bool inDebug, bool skipReset)
        {
            _fantasyUnitOfWork.GamesDataHelper.UpdateCurrentGameWeak(id, resetTeam, fk_AccounTeam, inDebug, skipReset).Wait();

            return Ok();
        }

        [HttpPost]
        [Route(nameof(UpdateRecurringJob))]
        public IActionResult UpdateRecurringJob()
        {
            //_unitOfWork.AccountTeam.ResetAccountTeamPlayer(3958, 58);
            //_unitOfWork.Save().Wait();

            //_fantasyUnitOfWork.GamesDataHelper.TransferAccountTeamPlayers(3958, 58, 51, 13, 5).Wait();
            //_unitOfWork.Save().Wait();

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
            RecurringJob.AddOrUpdate("RunAccountTeamsCalculations", () => _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(0, 0, null, null, false), "0 */8 * * *", TimeZoneInfo.Utc);

            // At 02:00 AM
            //RecurringJob.AddOrUpdate("UpdateAccountTeamGameWeakRanking", () => _fantasyUnitOfWork.AccountTeamCalc.RunUpdateAccountTeamGameWeakRanking(), "0 2 * * *", TimeZoneInfo.Utc);

            // At 03:00 AM
            //RecurringJob.AddOrUpdate("UpdateAccountTeamRanking", () => _fantasyUnitOfWork.AccountTeamCalc.RunUpdateAccountTeamRanking(), "0 3 * * *", TimeZoneInfo.Utc);

            // At 04:00 AM
            RecurringJob.AddOrUpdate("UpdatePrivateLeaguesRanking", () => _fantasyUnitOfWork.PrivateLeagueClac.RunPrivateLeaguesRanking(null, 0, false), "0 */14 * * *", TimeZoneInfo.Utc);

            // At 05:00 AM
            RecurringJob.AddOrUpdate("UpdateGames", () => _fantasyUnitOfWork.GamesDataHelper.RunUpdateGames(), "0 */6 * * *", TimeZoneInfo.Utc);

            // At 06:00 AM
            RecurringJob.AddOrUpdate("UpdateStandings", () => _fantasyUnitOfWork.StandingsDataHelper.RunUpdateStandings(), "0 */9 * * *", TimeZoneInfo.Utc);

        }

        private void WeeklyRecurringJob()
        {
            // At 07:00 AM, only on Friday
            RecurringJob.AddOrUpdate("UpdateTeams", () => _fantasyUnitOfWork.TeamDataHelper.RunUpdateTeams(), "0 7 * * 5", TimeZoneInfo.Utc);

            // At 08:00 AM, only on Friday
            RecurringJob.AddOrUpdate("UpdatePlayers", () => _fantasyUnitOfWork.PlayerDataHelper.RunUpdatePlayers(), "0 8 * * 5", TimeZoneInfo.Utc);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void RemoveOldRecurringJob()
        {
            _ = BackgroundJob.Enqueue(() => RemoveAccountTeamRecurringJob());
            _ = BackgroundJob.Enqueue(() => RemovePlayersRecurringJob());

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void RemoveAccountTeamRecurringJob()
        {
            List<int> accountTeams = _unitOfWork.AccountTeam.GetAccountTeams(new AccountTeamParameters(), false).Select(a => a.Id).ToList();
            foreach (int accountTeam in accountTeams)
            {
                RecurringJob.RemoveIfExists($"AccountTeamCalc-{accountTeam}");
            }

            List<int> accountTeamGameWeaks = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters(), false).Select(a => a.Id).ToList();
            foreach (int accountTeamGameWeak in accountTeamGameWeaks)
            {
                RecurringJob.RemoveIfExists($"AccountTeamGameWeakCalc-{accountTeamGameWeak}");
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void RemovePlayersRecurringJob()
        {
            Entities.CoreServicesModels.SeasonModels.GameWeakModel gameWeek = _unitOfWork.Season.GetCurrentGameWeak();

            List<int> players = _unitOfWork.Team.GetPlayers(new PlayerParameters
            {
                Fk_GameWeak = gameWeek.Id
            }, false).Select(a => a.Id).ToList();

            foreach (int player in players)
            {
                RecurringJob.RemoveIfExists($"PlayerGameWeekStatesCalc-{gameWeek.Id}-{player}");
            }
        }
    }
}
