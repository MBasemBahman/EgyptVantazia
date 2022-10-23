using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;
using IntegrationWith365.Entities.SquadsModels;

namespace FantasyLogic.DataMigration.TeamData
{
    public class PlayerPositionDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;

        public PlayerPositionDataHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
        }

        public void UpdatePositions()
        {
            List<string> teams = _unitOfWork.Team.GetTeams(new TeamParameters
            {
            }, otherLang: false).Select(a => a._365_TeamId).ToList();

            int minutes = 30;
            foreach (string _365_TeamId in teams)
            {
                _ = BackgroundJob.Schedule(() => UpdatePositions(_365_TeamId), TimeSpan.FromMinutes(minutes));
                minutes += 10;
            }
        }

        public async Task UpdatePositions(string _365_TeamId)
        {
            int _365_TeamIdVal = _365_TeamId.ParseToInt();
            SquadReturn squadsInArabic = await _365Services.GetSquads(new _365SquadsParameters
            {
                Competitors = _365_TeamIdVal,
                IsArabic = true,
            });

            SquadReturn squadsInEnglish = await _365Services.GetSquads(new _365SquadsParameters
            {
                Competitors = _365_TeamIdVal,
                IsArabic = false,
            });

            List<Position> positionsInArabic = squadsInArabic.Squads
                                                  .SelectMany(a => a.Athletes.Select(b => b.Position))
                                                  .GroupBy(a => a.Id)
                                                  .Where(a => a.Key > 0)
                                                  .Select(a => new Position
                                                  {
                                                      Id = a.Key,
                                                      Name = a.Select(a => a.Name).First()
                                                  })
                                                  .ToList();

            List<Position> positionsInEnglish = squadsInEnglish.Squads
                                                    .SelectMany(a => a.Athletes.Select(b => b.Position))
                                                    .GroupBy(a => a.Id)
                                                    .Where(a => a.Key > 0)
                                                    .Select(a => new Position
                                                    {
                                                        Id = a.Key,
                                                        Name = a.Select(a => a.Name).First()
                                                    })
                                                    .ToList();
            int minutes = 30;
            for (int i = 0; i < positionsInArabic.Count; i++)
            {
                _ = BackgroundJob.Schedule(() => UpdatePosition(positionsInArabic[i], positionsInEnglish[i]), TimeSpan.FromMinutes(minutes));
                minutes++;
            }
        }

        public async Task UpdatePosition(Position positionInArabic, Position positionInEnglish)
        {
            if (positionInArabic.Name.IsExisting() && positionInEnglish.Name.IsExisting())
            {
                _unitOfWork.Team.CreatePlayerPosition(new PlayerPosition
                {
                    Name = positionInArabic.Name,
                    _365_PositionId = positionInArabic.Id.ToString(),
                    PlayerPositionLang = new PlayerPositionLang
                    {
                        Name = positionInEnglish.Name
                    }
                });
            }
            else
            {
                _unitOfWork.Team.CreatePlayerPosition(new PlayerPosition
                {
                    Name = "مدرب",
                    _365_PositionId = positionInArabic.Id.ToString(),
                    PlayerPositionLang = new PlayerPositionLang
                    {
                        Name = "Coach"
                    }
                });
            }
            await _unitOfWork.Save();
        }
    }
}
