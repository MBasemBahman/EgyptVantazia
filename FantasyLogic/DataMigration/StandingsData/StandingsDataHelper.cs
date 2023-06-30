using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.StandingsModels;
using IntegrationWith365.Entities.StandingsModels;
using static Contracts.EnumData.DBModelsEnum;

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

        public void RunUpdateStandings(_365CompetitionsEnum _365CompetitionsEnum)
        {
            List<TeamForCalc> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
            }).Select(a => new TeamForCalc
            {
                Id = a.Id,
                _365_TeamId = a._365_TeamId
            }).ToList();

            SeasonModelForCalc season = _unitOfWork.Season.GetCurrentSeason(_365CompetitionsEnum);

            _ = BackgroundJob.Enqueue(() => UpdateSeasonStandings(_365CompetitionsEnum, teams, season._365_SeasonId.ParseToInt(), season.Id));
        }

        public async Task UpdateSeasonStandings(_365CompetitionsEnum _365CompetitionsEnum, List<TeamForCalc> teams, int _365_SeasonId, int fk_Season)
        {
            StandingsReturn standings = await _365Services.GetStandings(_365CompetitionsEnum, new _365StandingsParameters
            {
                SeasonNum = _365_SeasonId,
                IsArabic = true,
            });

            List<Row> rows = standings.Standings.SelectMany(a => a.Rows).ToList();

            string jobId = null;
            foreach (Row row in rows)
            {
                int fk_Team = teams.Where(a => a._365_TeamId == row.Competitor.Id.ToString())
                                   .Select(a => a.Id)
                                   .FirstOrDefault();

                jobId = jobId.IsExisting()
                    ? BackgroundJob.ContinueJobWith(jobId, () => UpdateStanding(row, fk_Season, fk_Team))
                    : BackgroundJob.Enqueue(() => UpdateStanding(row, fk_Season, fk_Team));
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

    public class TeamForCalc
    {
        public int Id { get; set; }

        public string _365_TeamId { get; set; }
    }
}
