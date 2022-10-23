using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;
using IntegrationWith365.Helpers;

namespace FantasyLogic.DataMigration.SeasonData
{
    public class GameWeakDataHelper
    {
        private readonly _365Services _365Services;
        private readonly UnitOfWork _unitOfWork;
        private readonly GamesHelper _gamesHelper;

        public GameWeakDataHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
            _gamesHelper = new GamesHelper(unitOfWork, _365Services);
        }

        public void UpdateGameWeaks()
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            _ = BackgroundJob.Schedule(() => UpdateGameWeaks(season.Id), TimeSpan.FromMinutes(5));
        }

        public void UpdateGameWeaks(int fk_season)
        {
            List<int> rounds = _gamesHelper.GetAllGames().Result.Select(a => a.RoundNum).Distinct().OrderBy(a => a).ToList();

            int minutes = 30;
            foreach (int round in rounds)
            {
                _ = BackgroundJob.Schedule(() => UpdateGameWeak(round, fk_season), TimeSpan.FromMinutes(minutes));
                minutes++;
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
