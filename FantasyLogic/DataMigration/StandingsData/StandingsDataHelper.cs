﻿using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.StandingsModels;
using IntegrationWith365.Entities.StandingsModels;

namespace FantasyLogic.DataMigration.StandingsData
{
    public class StandingsDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;

        public StandingsDataHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
        }

        public async Task UpdateStandings()
        {
            List<TeamDto> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
            }, otherLang: false).Select(a => new TeamDto
            {
                Id = a.Id,
                _365_TeamId = a._365_TeamId
            }).ToList();

            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            StandingsReturn standings = await _365Services.GetStandings(new _365StandingsParameters
            {
                SeasonNum = season._365_SeasonId.ParseToInt(),
                IsArabic = true,
            });

            List<Row> rows = standings.Standings.SelectMany(a => a.Rows).ToList();

            int minutes = 30;
            foreach (Row row in rows)
            {
                int fk_Team = teams.Where(a => a._365_TeamId == row.Competitor.Id.ToString())
                                   .Select(a => a.Id)
                                   .FirstOrDefault();

                _ = BackgroundJob.Schedule(() => UpdateStanding(row, season.Id, fk_Team), TimeSpan.FromMinutes(minutes));

                minutes++;
            }
        }

        public async Task UpdateStanding(Row row, int fk_Season, int fk_Team)
        {
            _unitOfWork.Standings.CreateStandings(new Standings
            {
                Fk_Season = fk_Season,
                Fk_Team = fk_Team,
                GamePlayed = row.GamePlayed,
                GamesWon = row.GamesWon,
                GamesLost = row.GamesLost,
                GamesEven = row.GamesEven,
                For = row.For,
                Against = row.Against,
                Ratio = row.Ratio,
                Strike = row.Strike,
                Position = row.Position,
            });

            await _unitOfWork.Save();
        }
    }

    public class TeamDto
    {
        public int Id { get; set; }

        public string _365_TeamId { get; set; }
    }
}
