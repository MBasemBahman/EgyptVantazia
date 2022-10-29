﻿using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.SeasonModels;
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

                jobId = jobId.IsExisting()
                    ? BackgroundJob.ContinueJobWith(jobId, () => UpdateGame(game, fk_Home, fk_Away, fk_GameWeak))
                    : BackgroundJob.Enqueue(() => UpdateGame(game, fk_Home, fk_Away, fk_GameWeak));
            }
        }

        public async Task UpdateGame(Games game, int fk_Home, int fk_Away, int fk_GameWeak)
        {
            _unitOfWork.Season.CreateTeamGameWeak(new TeamGameWeak
            {
                Fk_Away = fk_Away,
                Fk_Home = fk_Home,
                Fk_GameWeak = fk_GameWeak,
                StartTime = game.StartTimeVal,
                IsEnded = game.IsEnded,
                _365_MatchId = game.Id.ToString(),
                AwayScore = (int)game.AwayCompetitor.Score,
                HomeScore = (int)game.HomeCompetitor.Score,
            });
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
