using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;
using FantasyLogic.DataMigration.StandingsData;
using IntegrationWith365.Entities.SquadsModels;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogic.DataMigration.TeamData
{
    public class FormationPositionDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;

        public FormationPositionDataHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
        }

        public void RunUpdateFormationPositions(_365CompetitionsEnum _365CompetitionsEnum)
        {
            List<TeamForCalc> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
                _365_CompetitionsId = (int)_365CompetitionsEnum
            }).Select(a => new TeamForCalc
            {
                Id = a.Id,
                _365_TeamId = a._365_TeamId
            }).ToList();

            string jobId = null;
            foreach (TeamForCalc team in teams)
            {
                jobId = jobId.IsExisting()
                ? BackgroundJob.ContinueJobWith(jobId, () => UpdatePlayers(_365CompetitionsEnum, team, jobId))
                : BackgroundJob.Enqueue(() => UpdatePlayers(_365CompetitionsEnum, team, jobId));
            }
        }

        public async Task UpdatePlayers(_365CompetitionsEnum _365CompetitionsEnum, TeamForCalc team, string jobId)
        {
            int _365_TeamId = team._365_TeamId.ParseToInt();

            SquadReturn squadsInArabic = await _365Services.GetSquads(_365CompetitionsEnum, new _365SquadsParameters
            {
                Competitors = _365_TeamId,
                IsArabic = true,
            });

            SquadReturn squadsInEnglish = await _365Services.GetSquads(_365CompetitionsEnum, new _365SquadsParameters
            {
                Competitors = _365_TeamId,
                IsArabic = false,
            });

            List<Position> athletesInArabic = squadsInArabic.Squads.SelectMany(a => a.Athletes.Select(b => b.FormationPosition)).ToList();
            List<Position> athletesInEnglish = squadsInEnglish.Squads.SelectMany(a => a.Athletes.Select(b => b.FormationPosition)).ToList();

            for (int i = 0; i < athletesInArabic.Count; i++)
            {
                _unitOfWork.Team.CreateFormationPosition(new FormationPosition
                {
                    Name = athletesInArabic[i].Name,
                    _365_PositionId = athletesInArabic[i].Id.ToString(),
                    FormationPositionLang = new FormationPositionLang
                    {
                        Name = athletesInEnglish[i].Name,
                    }
                });

                _unitOfWork.Save().Wait();
            }
        }


    }
}
