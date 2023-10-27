using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;
using FantasyLogic.DataMigration.StandingsData;
using IntegrationWith365.Entities.SquadsModels;
using Microsoft.AspNetCore.Http.Features;
using static Contracts.EnumData.DBModelsEnum;

namespace FantasyLogic.DataMigration.TeamData
{
    public class PlayerDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;

        public PlayerDataHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
        }

        public void RunUpdatePlayers(_365CompetitionsEnum _365CompetitionsEnum)
        {
            List<TeamForCalc> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
                _365_CompetitionsId = (int)_365CompetitionsEnum
            }).Select(a => new TeamForCalc
            {
                Id = a.Id,
                _365_TeamId = a._365_TeamId
            }).ToList();

            List<PlayerPositionForCalc> positions = _unitOfWork.Team.GetPlayerPositions(new PlayerPositionParameters
            { }).Select(a => new PlayerPositionForCalc
            {
                Id = a.Id,
                _365_PositionId = a._365_PositionId
            }).ToList();

            List<PlayerPositionForCalc> formations = _unitOfWork.Team.GetFormationPositions(new FormationPositionParameters
            { }).Select(a => new PlayerPositionForCalc
            {
                Id = a.Id,
                _365_PositionId = a._365_PositionId
            }).ToList();

            _ = BackgroundJob.Enqueue(() => UpdateTeamsPlayers(_365CompetitionsEnum, teams, positions, formations));
        }

        public void UpdateTeamsPlayers(_365CompetitionsEnum _365CompetitionsEnum, List<TeamForCalc> teams, List<PlayerPositionForCalc> positions, List<PlayerPositionForCalc> formations)
        {
            string jobId = null;
            foreach (TeamForCalc team in teams)
            {
                jobId = jobId.IsExisting()
                    ? BackgroundJob.ContinueJobWith(jobId, () => UpdatePlayers(_365CompetitionsEnum, team, positions, formations, jobId))
                    : BackgroundJob.Enqueue(() => UpdatePlayers(_365CompetitionsEnum, team, positions, formations, jobId));
            }
        }

        public async Task UpdatePlayers(_365CompetitionsEnum _365CompetitionsEnum, TeamForCalc team, List<PlayerPositionForCalc> positions, List<PlayerPositionForCalc> formations, string jobId)
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

            List<Athlete> athletesInArabic = squadsInArabic.Squads.SelectMany(a => a.Athletes).ToList();
            List<Athlete> athletesInEnglish = squadsInEnglish.Squads.SelectMany(a => a.Athletes).ToList();

            for (int i = 0; i < athletesInArabic.Count; i++)
            {
                int fk_PlayerPosition = positions.Where(a => a._365_PositionId == athletesInArabic[i].Position.Id.ToString())
                                                 .Select(a => a.Id)
                                                 .FirstOrDefault();

                int fk_FormationPosition = formations.Where(a => a._365_PositionId == athletesInArabic[i].FormationPosition.Id.ToString())
                                                 .Select(a => a.Id)
                                                 .FirstOrDefault();

                if (fk_PlayerPosition != (int)PlayerPositionEnum.Coach)
                {

                    jobId = jobId.IsExisting()
                        ? BackgroundJob.ContinueJobWith(jobId, () => UpdatePlayer(athletesInArabic[i], athletesInEnglish[i], team.Id, fk_PlayerPosition, fk_FormationPosition))
                        : BackgroundJob.Enqueue(() => UpdatePlayer(athletesInArabic[i], athletesInEnglish[i], team.Id, fk_PlayerPosition, fk_FormationPosition));
                }
            }
        }

        public async Task UpdatePlayer(Athlete athleteInArabic, Athlete athleteInEnglish, int fk_Team, int fk_PlayerPosition, int fk_FormationPosition)
        {
            _unitOfWork.Team.CreatePlayer(new Player
            {
                Name = athleteInArabic.Name,
                ShortName = athleteInArabic.ShortName ?? athleteInEnglish.ShortName,
                Age = athleteInArabic.Age,
                Height = athleteInArabic.Height,
                Birthdate = athleteInArabic.Birthdate,
                PlayerNumber = athleteInArabic.JerseyNum.ToString(),
                _365_PlayerId = athleteInArabic.Id.ToString(),
                Fk_Team = fk_Team,
                Fk_PlayerPosition = fk_PlayerPosition,
                Fk_FormationPosition = fk_FormationPosition == 0 ? null : fk_FormationPosition,
                IsActive = false,
                PlayerLang = new PlayerLang
                {
                    Name = athleteInEnglish.Name,
                    ShortName = athleteInEnglish.ShortName ?? athleteInArabic.ShortName,
                }
            });
            await _unitOfWork.Save();
        }
    }

    public class PlayerPositionForCalc
    {
        public int Id { get; set; }

        public string _365_PositionId { get; set; }
    }
}
