using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.TeamModels;
using IntegrationWith365.Entities.GamesModels;
using IntegrationWith365.Entities.StandingsModels;
using static Contracts.EnumData.DBModelsEnum;

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

        public void RunUpdateTeams(_365CompetitionsEnum _365CompetitionsEnum)
        {
            SeasonModelForCalc season = _unitOfWork.Season.GetCurrentSeason(_365CompetitionsEnum);

            _ = BackgroundJob.Enqueue(() => UpdateSeasonTeams(_365CompetitionsEnum, season._365_SeasonId.ParseToInt(), season.Id));
        }

        public async Task UpdateSeasonTeams(_365CompetitionsEnum _365CompetitionsEnum, int _365_SeasonId, int fk_Season)
        {
            _unitOfWork.Team.UpdateTeamActivation((int)_365CompetitionsEnum, isActive: false);
            _unitOfWork.Save().Wait();

            StandingsReturn standingsInArabic = await _365Services.GetStandings(_365CompetitionsEnum, new _365StandingsParameters
            {
                SeasonNum = _365_SeasonId,
                IsArabic = true,
            });

            StandingsReturn standingsInEnglish = await _365Services.GetStandings(_365CompetitionsEnum, new _365StandingsParameters
            {
                SeasonNum = _365_SeasonId,
                IsArabic = false,
            });

            List<Competitor> competitorsInArabic = standingsInArabic.Standings.SelectMany(a => a.Rows.Select(b => b.Competitor)).ToList();
            List<Competitor> competitorsInEnglish = standingsInEnglish.Standings.SelectMany(a => a.Rows.Select(b => b.Competitor)).ToList();

            for (int i = 0; i < competitorsInArabic.Count; i++)
            {
                BackgroundJob.Enqueue(() => UpdateTeam(competitorsInArabic[i], competitorsInEnglish[i], fk_Season));
            }
        }

        public async Task UpdateTeam(Competitor competitorInArabic, Competitor competitorInEnglish, int fk_Season)
        {
            _unitOfWork.Team.CreateTeam(new Team
            {
                Name = competitorInArabic.Name,
                _365_TeamId = competitorInArabic.Id.ToString(),
                IsActive = true,
                Fk_Season = fk_Season,
                TeamLang = new TeamLang
                {
                    Name = competitorInEnglish.Name
                }
            });
            await _unitOfWork.Save();
        }
    }
}
