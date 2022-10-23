using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;
using IntegrationWith365.Helpers;

namespace FantasyLogic.DataMigration.SeasonData
{
    public class GameWeakDataHelper
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly GamesHelper _gamesHelper;

        public GameWeakDataHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            _unitOfWork = unitOfWork;
            _gamesHelper = new GamesHelper(unitOfWork, _365Services);
        }

        public void UpdateGameWeaks(int delayMinutes)
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            _ = BackgroundJob.Schedule(() => UpdateGameWeaks(season.Id, delayMinutes), TimeSpan.FromMinutes(delayMinutes));
        }

        public void UpdateGameWeaks(int fk_season, int delayMinutes)
        {
            List<int> rounds = _gamesHelper.GetAllGames().Result.Select(a => a.RoundNum).Distinct().OrderBy(a => a).ToList();

            delayMinutes *= delayMinutes;

            foreach (int round in rounds)
            {
                _ = BackgroundJob.Schedule(() => UpdateGameWeak(round, fk_season), TimeSpan.FromMinutes(delayMinutes));
                delayMinutes++;
            }
        }

        public async Task UpdateGameWeak(int round, int fk_Season)
        {
            _unitOfWork.Season.CreateGameWeak(new GameWeak
            {
                Name = round.ToString(),
                Fk_Season = fk_Season,
                _365_GameWeakId = round.ToString(),
                GameWeakLang = new GameWeakLang
                {
                    Name = round.ToString()
                }
            });
            await _unitOfWork.Save();
        }
    }
}
