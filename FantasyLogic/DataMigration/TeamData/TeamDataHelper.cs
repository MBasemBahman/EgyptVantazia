using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.TeamModels;
using IntegrationWith365.Entities.GamesModels;
using IntegrationWith365.Entities.StandingsModels;

namespace FantasyLogic.DataMigration.TeamData
{
    public class TeamDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;

        public TeamDataHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
        }

        public void RunUpdateTeams()
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            int _365_SeasonId = season._365_SeasonId.ParseToInt();

            _ = BackgroundJob.Enqueue(() => UpdateSeasonTeams(season._365_SeasonId.ParseToInt()));
        }

        public async Task UpdateSeasonTeams(int _365_SeasonId)
        {
            StandingsReturn standingsInArabic = await _365Services.GetStandings(new _365StandingsParameters
            {
                SeasonNum = _365_SeasonId,
                IsArabic = true,
            });

            StandingsReturn standingsInEnglish = await _365Services.GetStandings(new _365StandingsParameters
            {
                SeasonNum = _365_SeasonId,
                IsArabic = false,
            });

            List<Competitor> competitorsInArabic = standingsInArabic.Standings.SelectMany(a => a.Rows.Select(b => b.Competitor)).ToList();
            List<Competitor> competitorsInEnglish = standingsInEnglish.Standings.SelectMany(a => a.Rows.Select(b => b.Competitor)).ToList();

            string jobId = null;

            for (int i = 0; i < competitorsInArabic.Count; i++)
            {
                jobId = jobId.IsExisting()
                    ? BackgroundJob.ContinueJobWith(jobId, () => UpdateTeam(competitorsInArabic[i], competitorsInEnglish[i]))
                    : BackgroundJob.Enqueue(() => UpdateTeam(competitorsInArabic[i], competitorsInEnglish[i]));
            }
        }

        public async Task UpdateTeam(Competitor competitorInArabic, Competitor competitorInEnglish)
        {
            _unitOfWork.Team.CreateTeam(new Team
            {
                Name = competitorInArabic.Name,
                _365_TeamId = competitorInArabic.Id.ToString(),
                TeamLang = new TeamLang
                {
                    Name = competitorInEnglish.Name
                }
            });
            await _unitOfWork.Save();
        }
    }
}
