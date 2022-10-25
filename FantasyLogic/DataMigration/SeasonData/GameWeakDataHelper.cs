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

        public void RunUpdateGameWeaks(int delayMinutes)
        {
            SeasonModel season = _unitOfWork.Season.GetCurrentSeason();

            _ = BackgroundJob.Schedule(() => UpdateSeasonGameWeaks(season.Id, season._365_SeasonId.ParseToInt(), delayMinutes), TimeSpan.FromMinutes(delayMinutes));
        }

        public void UpdateSeasonGameWeaks(int fk_season, int _365_SeasonId, int delayMinutes)
        {
            List<int> rounds = _gamesHelper.GetAllGames(_365_SeasonId).Result.Select(a => a.RoundNum).Distinct().OrderBy(a => a).ToList();

            foreach (int round in rounds)
            {
                _ = BackgroundJob.Schedule(() => UpdateGameWeak(round, _365_SeasonId), TimeSpan.FromMinutes(delayMinutes));
            }

            _ = BackgroundJob.Schedule(() => UpdateCurrentGameWeak(fk_season, _365_SeasonId), TimeSpan.FromMinutes(delayMinutes * rounds.Count));
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

        public async Task UpdateCurrentGameWeak(int fk_Season, int _365_SeasonId)
        {
            int round = _gamesHelper.GetCurrentRound(_365_SeasonId);
            if (round > 0)
            {
                GameWeak gameWeak = await _unitOfWork.Season.FindGameWeakby365Id(round.ToString(), fk_Season, trackChanges: true);
                if (gameWeak != null)
                {
                    gameWeak.IsCurrent = true;
                    await _unitOfWork.Save();
                }
            }
        }
    }
}
