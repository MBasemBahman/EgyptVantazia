using CoreServices;
using IntegrationWith365.Entities.GamesModels;
using IntegrationWith365.Parameters;

namespace IntegrationWith365.Helpers
{
    public class GamesHelper
    {
        private readonly _365Services _365Services;
        protected readonly UnitOfWork _unitOfWork;

        public GamesHelper(UnitOfWork unitOfWork, _365Services _365Services)
        {
            this._365Services = _365Services;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Games>> GetAllGames(int _365_SeasonId)
        {
            List<Games> games = new();

            games.AddRange(await GetPrevGames(_365_SeasonId));
            games.AddRange(await GetNextGames(_365_SeasonId));

            return games;
        }

        public async Task<List<Games>> GetPrevGames(int _365_SeasonId)
        {
            List<Games> games = new();

            GamesReturn gamesReturn = await _365Services.GetGamesResults(new _365Parameters());

            if (gamesReturn.Games != null && gamesReturn.Games.Any())
            {
                games.AddRange(gamesReturn.Games);

                if (gamesReturn.Paging != null)
                {
                    games.AddRange(await GetGames(gamesReturn.Paging.PreviousAfterGame));
                }
            }

            return games.Where(a => a.SeasonNum == _365_SeasonId && a.IsEnded == true).ToList();
        }

        public async Task<List<Games>> GetNextGames(int _365_SeasonId)
        {
            List<Games> games = new();

            GamesReturn gamesReturn = await _365Services.GetGamesFixtures(new _365Parameters());

            if (gamesReturn.Games != null && gamesReturn.Games.Any())
            {
                games.AddRange(gamesReturn.Games);

                if (gamesReturn.Paging != null)
                {
                    games.AddRange(await GetGames(gamesReturn.Paging.NextAfterGame));
                }
            }

            return games.Where(a => a.SeasonNum == _365_SeasonId && a.IsEnded == false).ToList();
        }

        public int GetCurrentRound(int _365_SeasonId)
        {
            return GetPrevGames(_365_SeasonId).Result.Select(a => a.RoundNum).OrderByDescending(a => a).FirstOrDefault();
        }

        public int GetNextRound(int _365_SeasonId)
        {
            return GetCurrentRound(_365_SeasonId) + 1;
        }

        public async Task<List<Games>> GetGames(int _365_AfterGameStartId)
        {
            List<Games> games = new();

            while (_365_AfterGameStartId > 0)
            {
                GamesReturn gamesReturn = await _365Services.GetGames(new _365GamesParameters
                {
                    Aftergame = _365_AfterGameStartId
                });

                if (gamesReturn.Games != null && gamesReturn.Games.Any())
                {
                    if (gamesReturn.Paging != null)
                    {
                        _365_AfterGameStartId = gamesReturn.Paging.PreviousAfterGame == 0 ? gamesReturn.Paging.NextAfterGame : gamesReturn.Paging.PreviousAfterGame;
                    }

                    games.AddRange(gamesReturn.Games);
                }
                else
                {
                    _365_AfterGameStartId = 0;
                }
            }

            return games;
        }
    }
}
