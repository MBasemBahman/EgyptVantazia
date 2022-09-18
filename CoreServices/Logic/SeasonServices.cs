using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.SeasonModels;

namespace CoreServices.Logic
{
    public class SeasonServices
    {
        private readonly RepositoryManager _repository;

        public SeasonServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region Season Services
        public IQueryable<SeasonModel> GetSeasons(SeasonParameters parameters,
                bool otherLang)
        {
            return _repository.Season
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new SeasonModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           _365_SeasonId = a._365_SeasonId,
                           Name = otherLang ? a.SeasonLang.Name : a.Name,
                           ImageUrl = a.StorageUrl + a.ImageUrl
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<SeasonModel>> GetSeasonPaged(
                  SeasonParameters parameters,
                  bool otherLang)
        {
            return await PagedList<SeasonModel>.ToPagedList(GetSeasons(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Season> FindSeasonbyId(int id, bool trackChanges)
        {
            return await _repository.Season.FindById(id, trackChanges);
        }

        public void CreateSeason(Season Season)
        {
            _repository.Season.Create(Season);
        }

        public async Task DeleteSeason(int id)
        {
            Season Season = await FindSeasonbyId(id, trackChanges: true);
            _repository.Season.Delete(Season);
        }

        public SeasonModel GetSeasonbyId(int id, bool otherLang)
        {
            return GetSeasons(new SeasonParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetSeasonCount()
        {
            return _repository.Season.Count();
        }

        public SeasonModel GetCurrentSeason()
        {
            return GetSeasons(new SeasonParameters(), otherLang: false)
                   .OrderByDescending(x => x.Id)
                   .FirstOrDefault();
        }

        #endregion

        #region GameWeak Services
        public IQueryable<GameWeakModel> GetGameWeaks(GameWeakParameters parameters,
                bool otherLang)
        {
            return _repository.GameWeak
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new GameWeakModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           _365_GameWeakId = a._365_GameWeakId,
                           Name = otherLang ? a.GameWeakLang.Name : a.Name,
                           Fk_Season = a.Fk_Season,
                           Season = new SeasonModel
                           {
                               Name = otherLang ? a.Season.SeasonLang.Name : a.Season.Name,
                               ImageUrl = a.Season.StorageUrl + a.Season.ImageUrl,
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<GameWeakModel>> GetGameWeakPaged(
                  GameWeakParameters parameters,
                  bool otherLang)
        {
            return await PagedList<GameWeakModel>.ToPagedList(GetGameWeaks(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<GameWeak> FindGameWeakbyId(int id, bool trackChanges)
        {
            return await _repository.GameWeak.FindById(id, trackChanges);
        }

        public void CreateGameWeak(GameWeak GameWeak)
        {
            _repository.GameWeak.Create(GameWeak);
        }

        public async Task DeleteGameWeak(int id)
        {
            GameWeak GameWeak = await FindGameWeakbyId(id, trackChanges: true);
            _repository.GameWeak.Delete(GameWeak);
        }

        public GameWeakModel GetGameWeakbyId(int id, bool otherLang)
        {
            return GetGameWeaks(new GameWeakParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetGameWeakCount()
        {
            return _repository.GameWeak.Count();
        }
        #endregion

        #region TeamGameWeak Services
        public IQueryable<TeamGameWeakModel> GetTeamGameWeaks(TeamGameWeakParameters parameters,
                bool otherLang)
        {
            return _repository.TeamGameWeak
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new TeamGameWeakModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           AwayScore = a.AwayScore,
                           Fk_Away = a.Fk_Away,
                           Fk_GameWeak = a.Fk_GameWeak,
                           Fk_Home = a.Fk_Home,
                           HomeScore = a.HomeScore,
                           StartTime = a.StartTime,
                           _365_MatchId = a._365_MatchId,
                           _365_MatchUpId = a._365_MatchId,
                           Away = new TeamModel
                           {
                               Name = otherLang ? a.Away.TeamLang.Name : a.Away.Name,
                               ImageUrl = a.Away.StorageUrl + a.Away.ImageUrl,
                           },
                           Home = new TeamModel
                           {
                               Name = otherLang ? a.Home.TeamLang.Name : a.Home.Name,
                               ImageUrl = a.Home.StorageUrl + a.Home.ImageUrl,
                           },
                           GameWeak = new GameWeakModel
                           {
                               Name = otherLang ? a.GameWeak.GameWeakLang.Name : a.GameWeak.Name
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<TeamGameWeakModel>> GetTeamGameWeakPaged(
                  TeamGameWeakParameters parameters,
                  bool otherLang)
        {
            return await PagedList<TeamGameWeakModel>.ToPagedList(GetTeamGameWeaks(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<TeamGameWeak> FindTeamGameWeakbyId(int id, bool trackChanges)
        {
            return await _repository.TeamGameWeak.FindById(id, trackChanges);
        }

        public void CreateTeamGameWeak(TeamGameWeak TeamGameWeak)
        {
            _repository.TeamGameWeak.Create(TeamGameWeak);
        }

        public async Task DeleteTeamGameWeak(int id)
        {
            TeamGameWeak TeamGameWeak = await FindTeamGameWeakbyId(id, trackChanges: true);
            _repository.TeamGameWeak.Delete(TeamGameWeak);
        }

        public TeamGameWeakModel GetTeamGameWeakbyId(int id, bool otherLang)
        {
            return GetTeamGameWeaks(new TeamGameWeakParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetTeamGameWeakCount()
        {
            return _repository.TeamGameWeak.Count();
        }
        #endregion
    }
}
