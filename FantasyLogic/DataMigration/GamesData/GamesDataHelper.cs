using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.SeasonModels;
using FantasyLogic.DataMigration.PlayerScoreData;
using FantasyLogic.DataMigration.StandingsData;
using IntegrationWith365.Entities.GamesModels;
using IntegrationWith365.Helpers;

namespace FantasyLogic.DataMigration.GamesData
{
    public class GamesDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;
        private readonly GamesHelper _gamesHelper;

        public GamesDataHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
            _gamesHelper = new GamesHelper(unitOfWork, _365Services);
        }

        public void RunUpdateGames()
        {
            List<TeamDto> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
            }, otherLang: false).Select(a => new TeamDto
            {
                Id = a.Id,
                _365_TeamId = a._365_TeamId
            }).ToList();

            //UpdateGames(teams);
            _ = BackgroundJob.Enqueue(() => UpdateGames(teams));
        }

        public void UpdateGames(List<TeamDto> teams)
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            List<GameWeakDto> gameWeaks = _unitOfWork.Season.GetGameWeaks(new GameWeakParameters
            {
                Fk_Season = season.Id
            }, otherLang: false).Select(a => new GameWeakDto
            {
                Id = a.Id,
                _365_GameWeakId = a._365_GameWeakId
            }).ToList();

            List<Games> games = _gamesHelper.GetAllGames(season._365_SeasonId.ParseToInt()).Result.OrderBy(a => a.RoundNum).ThenBy(a => a.StartTimeVal).ToList();

            string jobId = null;

            foreach (Games game in games)
            {
                int fk_Home = teams.Where(a => a._365_TeamId == game.HomeCompetitor.Id.ToString()).Select(a => a.Id).FirstOrDefault();
                int fk_Away = teams.Where(a => a._365_TeamId == game.AwayCompetitor.Id.ToString()).Select(a => a.Id).FirstOrDefault();
                int fk_GameWeak = gameWeaks.Where(a => a._365_GameWeakId == game.RoundNum.ToString()).Select(a => a.Id).FirstOrDefault();

                if (fk_GameWeak > 0)
                {
                    //UpdateGame(game, fk_Home, fk_Away, fk_GameWeak).Wait();

                    jobId = jobId.IsExisting()
                        ? BackgroundJob.ContinueJobWith(jobId, () => UpdateGame(game, fk_Home, fk_Away, fk_GameWeak))
                        : BackgroundJob.Enqueue(() => UpdateGame(game, fk_Home, fk_Away, fk_GameWeak));
                }
            }

            //UpdateGameWeakDeadline(season.Id).Wait();

            jobId = jobId.IsExisting()
                    ? BackgroundJob.ContinueJobWith(jobId, () => UpdateGameWeakDeadline(season.Id))
                    : BackgroundJob.Enqueue(() => UpdateGameWeakDeadline(season.Id));
        }

        public async Task UpdateGame(Games game, int fk_Home, int fk_Away, int fk_GameWeak)
        {
            var startTime = game.StartTimeVal.AddHours(2);

            _unitOfWork.Season.CreateTeamGameWeak(new TeamGameWeak
            {
                Fk_Away = fk_Away,
                Fk_Home = fk_Home,
                Fk_GameWeak = fk_GameWeak,
                StartTime = startTime,
                IsEnded = game.IsEnded,
                _365_MatchId = game.Id.ToString(),
                AwayScore = (int)game.AwayCompetitor.Score,
                HomeScore = (int)game.HomeCompetitor.Score,
            });
            await _unitOfWork.Save();

            if (startTime > DateTime.UtcNow.AddHours(2))
            {
                StandingsDataHelper standingsDataHelper = new(_unitOfWork, _365Services);
                GameResultDataHelper gameResultDataHelper = new(_unitOfWork, _365Services);

                string recurringTime = startTime.AddHours(-2).ToCronExpression(startTime.AddMinutes(30), 10);

                RecurringJob.AddOrUpdate("UpdateStandings-" + game.Id.ToString(), () => standingsDataHelper.RunUpdateStandings(), recurringTime, TimeZoneInfo.Utc);

                RecurringJob.AddOrUpdate("UpdateGameResult-" + game.Id.ToString(), () => gameResultDataHelper.RunUpdateGameResult(new TeamGameWeakParameters { _365_MatchId = game.Id.ToString() }), recurringTime, TimeZoneInfo.Utc);
            }
        }

        public async Task UpdateGameWeakDeadline(int fk_Season)
        {
            var teamGameWeaks = _unitOfWork.Season.GetTeamGameWeaks(new TeamGameWeakParameters
            {
                Fk_Season = fk_Season
            }, otherLang: false)
                .GroupBy(a => a.Fk_GameWeak)
                .Select(a => new
                {
                    Fk_GameWeak = a.Key,
                    Deadline = a.Select(b => b.StartTime).OrderBy(b => b).FirstOrDefault()
                })
                .ToList();

            foreach (var teamGameWeak in teamGameWeaks)
            {
                if (teamGameWeak.Deadline > DateTime.MinValue)
                {
                    DateTime deadline = teamGameWeak.Deadline.AddMinutes(-90);

                    GameWeak gameWeak = await _unitOfWork.Season.FindGameWeakbyId(teamGameWeak.Fk_GameWeak, trackChanges: true);

                    if (gameWeak.JobId.IsExisting())
                    {
                        _ = BackgroundJob.Delete(gameWeak.JobId);
                    }

                    if (deadline > DateTime.UtcNow)
                    {
                        gameWeak.JobId = BackgroundJob.Schedule(() => UpdateCurrentGameWeak(gameWeak.Id), deadline.AddHours(-2));
                    }

                    gameWeak.Deadline = deadline;

                    await _unitOfWork.Save();
                }
            }
        }

        public async Task UpdateCurrentGameWeak(int id)
        {
            _unitOfWork.Season.ResetCurrentGameWeaks();
            await _unitOfWork.Save();

            GameWeak gameWeak = await _unitOfWork.Season.FindGameWeakbyId(id, trackChanges: false);
            gameWeak.IsCurrent = true;
            await _unitOfWork.Save();
        }
    }

    public class GameWeakDto
    {
        public int Id { get; set; }

        public string _365_GameWeakId { get; set; }
    }

    public class TeamDto
    {
        public int Id { get; set; }

        public string _365_TeamId { get; set; }
    }
}
