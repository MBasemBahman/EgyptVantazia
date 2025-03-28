﻿using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.SeasonModels;
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
            RecurringJob.AddOrUpdate($"Remove-Update-Matchs", () => RemoveUpdateMatchRecurringJob(), CronExpression.EveryHour(1));

            RecurringJob.AddOrUpdate($"Run-AccountTeams-Calculations-{_365CompetitionsEnum.Egypt}", () => _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(_365CompetitionsEnum.Egypt, 0, 0, null, null, false), CronExpression.EveryHour(12));
            RecurringJob.AddOrUpdate($"Run-AccountTeams-Calculations-{_365CompetitionsEnum.KSA}", () => _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(_365CompetitionsEnum.KSA, 0, 0, null, null, false), CronExpression.EveryHour(12));
            RecurringJob.AddOrUpdate($"Run-AccountTeams-Calculations-{_365CompetitionsEnum.EPL}", () => _fantasyUnitOfWork.AccountTeamCalc.RunAccountTeamsCalculations(_365CompetitionsEnum.EPL, 0, 0, null, null, false), CronExpression.EveryHour(12));

            RecurringJob.AddOrUpdate($"Update-PrivateLeagues-Ranking-{_365CompetitionsEnum.Egypt}", () => _fantasyUnitOfWork.PrivateLeagueClac.RunPrivateLeaguesRanking(_365CompetitionsEnum.Egypt, null, 0, false), CronExpression.EveryDay(hour: 9));
            RecurringJob.AddOrUpdate($"Update-PrivateLeagues-Ranking-{_365CompetitionsEnum.KSA}", () => _fantasyUnitOfWork.PrivateLeagueClac.RunPrivateLeaguesRanking(_365CompetitionsEnum.KSA, null, 0, false), CronExpression.EveryDay(hour: 9));
            RecurringJob.AddOrUpdate($"Update-PrivateLeagues-Ranking-{_365CompetitionsEnum.EPL}", () => _fantasyUnitOfWork.PrivateLeagueClac.RunPrivateLeaguesRanking(_365CompetitionsEnum.EPL, null, 0, false), CronExpression.EveryDay(hour: 9));

            RecurringJob.AddOrUpdate($"Update-Games-{_365CompetitionsEnum.Egypt}", () => _fantasyUnitOfWork.GamesDataHelper.RunUpdateGames(_365CompetitionsEnum.Egypt), CronExpression.EveryDay(hour: 5));
            RecurringJob.AddOrUpdate($"Update-Games-{_365CompetitionsEnum.KSA}", () => _fantasyUnitOfWork.GamesDataHelper.RunUpdateGames(_365CompetitionsEnum.KSA), CronExpression.EveryDay(hour: 5));
            RecurringJob.AddOrUpdate($"Update-Games-{_365CompetitionsEnum.EPL}", () => _fantasyUnitOfWork.GamesDataHelper.RunUpdateGames(_365CompetitionsEnum.EPL), CronExpression.EveryDay(hour: 5));

            RecurringJob.AddOrUpdate($"Update-Standings-{_365CompetitionsEnum.Egypt}", () => _fantasyUnitOfWork.StandingsDataHelper.RunUpdateStandings(_365CompetitionsEnum.Egypt), CronExpression.EveryDay(hour: 6));
            RecurringJob.AddOrUpdate($"Update-Standings-{_365CompetitionsEnum.KSA}", () => _fantasyUnitOfWork.StandingsDataHelper.RunUpdateStandings(_365CompetitionsEnum.KSA), CronExpression.EveryDay(hour: 6));
            RecurringJob.AddOrUpdate($"Update-Standings-{_365CompetitionsEnum.EPL}", () => _fantasyUnitOfWork.StandingsDataHelper.RunUpdateStandings(_365CompetitionsEnum.EPL), CronExpression.EveryDay(hour: 6));

            RecurringJob.AddOrUpdate($"Update-Players-{_365CompetitionsEnum.Egypt}", () => _fantasyUnitOfWork.PlayerDataHelper.RunUpdatePlayers(_365CompetitionsEnum.Egypt), CronExpression.EveryDay(hour: 7));
            RecurringJob.AddOrUpdate($"Update-Players-{_365CompetitionsEnum.KSA}", () => _fantasyUnitOfWork.PlayerDataHelper.RunUpdatePlayers(_365CompetitionsEnum.KSA), CronExpression.EveryDay(hour: 7));
            RecurringJob.AddOrUpdate($"Update-Players-{_365CompetitionsEnum.EPL}", () => _fantasyUnitOfWork.PlayerDataHelper.RunUpdatePlayers(_365CompetitionsEnum.EPL), CronExpression.EveryDay(hour: 7));

            //EPL
            RecurringJob.AddOrUpdate($"Update-AccountTeam-GameWeak-Ranking-{_365CompetitionsEnum.EPL}", () => _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamGameWeakRanking(_365CompetitionsEnum.EPL, 0, 0), CronExpression.EveryDay(hour: 6));
            RecurringJob.AddOrUpdate($"Update-AccountTeam-Ranking-{_365CompetitionsEnum.EPL}", () => _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamRanking(_365CompetitionsEnum.EPL), CronExpression.EveryDay(hour: 6));

            //KSA
            RecurringJob.AddOrUpdate($"Update-AccountTeam-GameWeak-Ranking-{_365CompetitionsEnum.KSA}", () => _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamGameWeakRanking(_365CompetitionsEnum.KSA, 0, 0), CronExpression.EveryDay(hour: 6));
            RecurringJob.AddOrUpdate($"Update-AccountTeam-Ranking-{_365CompetitionsEnum.KSA}", () => _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamRanking(_365CompetitionsEnum.KSA), CronExpression.EveryDay(hour: 6));

            //Egypt
            RecurringJob.AddOrUpdate($"Update-AccountTeam-GameWeak-Ranking-{_365CompetitionsEnum.Egypt}", () => _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamGameWeakRanking(_365CompetitionsEnum.Egypt, 0, 0), CronExpression.EveryDay(hour: 6));
            RecurringJob.AddOrUpdate($"Update-AccountTeam-Ranking-{_365CompetitionsEnum.Egypt}", () => _fantasyUnitOfWork.AccountTeamCalc.UpdateAccountTeamRanking(_365CompetitionsEnum.Egypt), CronExpression.EveryDay(hour: 6));


        }

        private void WeeklyRecurringJob()
        {
            RecurringJob.AddOrUpdate($"Update-Teams-{_365CompetitionsEnum.Egypt}", () => _fantasyUnitOfWork.TeamDataHelper.RunUpdateTeams(_365CompetitionsEnum.Egypt), CronExpression.EveryWeek(DayOfWeek.Saturday));
            RecurringJob.AddOrUpdate($"Update-Teams-{_365CompetitionsEnum.KSA}", () => _fantasyUnitOfWork.TeamDataHelper.RunUpdateTeams(_365CompetitionsEnum.KSA), CronExpression.EveryWeek(DayOfWeek.Saturday));
            RecurringJob.AddOrUpdate($"Update-Teams-{_365CompetitionsEnum.EPL}", () => _fantasyUnitOfWork.TeamDataHelper.RunUpdateTeams(_365CompetitionsEnum.EPL), CronExpression.EveryWeek(DayOfWeek.Saturday));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void RemoveOldRecurringJob()
        {
            _ = BackgroundJob.Enqueue(() => RemoveAccountTeamRecurringJob());

            _ = BackgroundJob.Enqueue(() => RemovePlayersRecurringJob(_365CompetitionsEnum.Egypt));
            _ = BackgroundJob.Enqueue(() => RemovePlayersRecurringJob(_365CompetitionsEnum.KSA));
            _ = BackgroundJob.Enqueue(() => RemovePlayersRecurringJob(_365CompetitionsEnum.EPL));
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
        public void RemoveUpdateMatchRecurringJob()
        {
            var matches = _unitOfWork.Season.GetTeamGameWeaks(new TeamGameWeakParameters
            {
                FromTime = DateTime.UtcNow.AddDays(-1),
                ToTime = DateTime.UtcNow,
                IsEnded = true
            }).ToList();
            foreach (TeamGameWeak match in matches)
            {
                if (match.StartTime.AddMinutes(210) < DateTime.UtcNow)
                {
                    RecurringJob.RemoveIfExists($"UpdateMatch-{match._365_MatchId}");
                }
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
