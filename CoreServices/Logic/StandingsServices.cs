using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.StandingsModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.StandingsModels;

namespace CoreServices.Logic
{
    public class StandingsServices
    {
        private readonly RepositoryManager _repository;

        public StandingsServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region Standings Services
        public IQueryable<StandingsModel> GetStandings(StandingsParameters parameters,
                bool otherLang)
        {
            return _repository.Standings
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new StandingsModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           GamePlayed = a.GamePlayed,
                           GamesWon = a.GamesWon,
                           GamesLost = a.GamesLost,
                           GamesEven = a.GamesEven,
                           For = a.For,
                           Against = a.Against,
                           Ratio = a.Ratio,
                           Fk_Season = a.Fk_Season,
                           Fk_Team = a.Fk_Team,
                           Season = new SeasonModel
                           {
                               Name = otherLang ? a.Season.SeasonLang.Name : a.Season.Name,
                               ImageUrl = a.Season.StorageUrl + a.Season.ImageUrl,
                               _365_SeasonId = a.Season._365_SeasonId
                           },
                           Team = new TeamModel
                           {
                               Name = otherLang ? a.Team.TeamLang.Name : a.Team.Name,
                               ImageUrl = a.Team.StorageUrl + a.Team.ImageUrl,
                               _365_TeamId = a.Team._365_TeamId
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<StandingsModel>> GetStandingsPaged(
                  StandingsParameters parameters,
                  bool otherLang)
        {
            return await PagedList<StandingsModel>.ToPagedList(GetStandings(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Standings> FindStandingsbyId(int id, bool trackChanges)
        {
            return await _repository.Standings.FindById(id, trackChanges);
        }

        public async Task<Standings> FindBySeasonAndTeam(int fk_Season, int fk_Team, bool trackChanges)
        {
            return await _repository.Standings.FindBySeasonAndTeam(fk_Season, fk_Team, trackChanges);
        }

        public void CreateStandings(Standings Standings)
        {
            _repository.Standings.Create(Standings);
        }

        public async Task DeleteStandings(int id)
        {
            Standings Standings = await FindStandingsbyId(id, trackChanges: true);
            _repository.Standings.Delete(Standings);
        }

        public StandingsModel GetStandingsbyId(int id, bool otherLang)
        {
            return GetStandings(new StandingsParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetStandingsCount()
        {
            return _repository.Standings.Count();
        }
        #endregion

    }
}
