using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;
using IntegrationWith365.Entities.SquadsModels;

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

        public void RunUpdatePlayers(TeamParameters parameters, int delayMinutes)
        {
            List<TeamModel> teams = _unitOfWork.Team.GetTeams(parameters, otherLang: false).ToList();

            _ = BackgroundJob.Schedule(() => UpdateTeamsPlayers(teams, delayMinutes), TimeSpan.FromMinutes(delayMinutes));
        }

        public void UpdateTeamsPlayers(List<TeamModel> teams, int delayMinutes)
        {
            List<PlayerPositionDto> positions = _unitOfWork.Team.GetPlayerPositions(new PlayerPositionParameters
            {
            }, otherLang: false).Select(a => new PlayerPositionDto
            {
                Id = a.Id,
                _365_PositionId = a._365_PositionId
            }).ToList();

            foreach (TeamModel team in teams)
            {
                _ = BackgroundJob.Schedule(() => UpdatePlayers(team, positions, delayMinutes), TimeSpan.FromMinutes(delayMinutes));
                
            }
        }

        public async Task UpdatePlayers(TeamModel team, List<PlayerPositionDto> positions, int delayMinutes)
        {
            int _365_TeamId = team._365_TeamId.ParseToInt();

            SquadReturn squadsInArabic = await _365Services.GetSquads(new _365SquadsParameters
            {
                Competitors = _365_TeamId,
                IsArabic = true,
            });

            SquadReturn squadsInEnglish = await _365Services.GetSquads(new _365SquadsParameters
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

                _ = BackgroundJob.Schedule(() => UpdatePlayer(athletesInArabic[i], athletesInEnglish[i], team.Id, fk_PlayerPosition), TimeSpan.FromMinutes(delayMinutes));
                
            }
        }

        public async Task UpdatePlayer(Athlete athleteInArabic, Athlete athleteInEnglish, int fk_Team, int fk_PlayerPosition)
        {
            _unitOfWork.Team.CreatePlayer(new Player
            {
                Name = athleteInArabic.Name,
                ShortName = athleteInArabic.ShortName ?? athleteInEnglish.ShortName,
                Age = athleteInArabic.Age,
                PlayerNumber = athleteInArabic.JerseyNum.ToString(),
                _365_PlayerId = athleteInArabic.Id.ToString(),
                Fk_Team = fk_Team,
                Fk_PlayerPosition = fk_PlayerPosition,
                PlayerLang = new PlayerLang
                {
                    Name = athleteInEnglish.Name,
                    ShortName = athleteInEnglish.ShortName ?? athleteInArabic.ShortName,
                }
            });
            await _unitOfWork.Save();
        }
    }

    public class PlayerPositionDto
    {
        public int Id { get; set; }

        public string _365_PositionId { get; set; }
    }
}
