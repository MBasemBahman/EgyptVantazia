using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.MatchStatisticModels;

namespace CoreServices.Logic
{
    public class MatchStatisticServices
    {
        private readonly RepositoryManager _repository;
        public MatchStatisticServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region StatisticCategory Services
        public IQueryable<StatisticCategoryModel> GetStatisticCategorys(StatisticCategoryParameters parameters, bool otherLang)
        {
            return _repository.StatisticCategory
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new StatisticCategoryModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Name = otherLang ? a.StatisticCategoryLang.Name : a.Name,
                           _365_Id = a._365_Id
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<StatisticCategoryModel>> GetStatisticCategorysPaged(
                  StatisticCategoryParameters parameters, bool otherLang)
        {
            return await PagedList<StatisticCategoryModel>.ToPagedList(GetStatisticCategorys(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public Dictionary<string, string> GetStatisticCategorysLookUp(StatisticCategoryParameters parameters, bool otherLang)
        {
            return GetStatisticCategorys(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }

        public async Task<StatisticCategory> FindStatisticCategorybyId(int id, bool trackChanges)
        {
            return await _repository.StatisticCategory.FindById(id, trackChanges);
        }

        public void CreateStatisticCategory(StatisticCategory entiy)
        {
            _repository.StatisticCategory.Create(entiy);
        }

        public async Task DeleteStatisticCategory(int id)
        {
            StatisticCategory entity = await FindStatisticCategorybyId(id, trackChanges: true);
            _repository.StatisticCategory.Delete(entity);
        }

        public StatisticCategoryModel GetStatisticCategorybyId(int id, bool otherLang)
        {
            return GetStatisticCategorys(new StatisticCategoryParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetStatisticCategoryCount()
        {
            return _repository.StatisticCategory.Count();
        }
        #endregion

        #region StatisticScore Services
        public IQueryable<StatisticScoreModel> GetStatisticScores(StatisticScoreParameters parameters, bool otherLang)
        {
            return _repository.StatisticScore
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new StatisticScoreModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Name = otherLang ? a.StatisticScoreLang.Name : a.Name,
                           _365_Id = a._365_Id
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<StatisticScoreModel>> GetStatisticScoresPaged(
                  StatisticScoreParameters parameters, bool otherLang)
        {
            return await PagedList<StatisticScoreModel>.ToPagedList(GetStatisticScores(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public Dictionary<string, string> GetStatisticScoresLookUp(StatisticScoreParameters parameters, bool otherLang)
        {
            return GetStatisticScores(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }

        public async Task<StatisticScore> FindStatisticScorebyId(int id, bool trackChanges)
        {
            return await _repository.StatisticScore.FindById(id, trackChanges);
        }

        public void CreateStatisticScore(StatisticScore entiy)
        {
            _repository.StatisticScore.Create(entiy);
        }

        public async Task DeleteStatisticScore(int id)
        {
            StatisticScore entity = await FindStatisticScorebyId(id, trackChanges: true);
            _repository.StatisticScore.Delete(entity);
        }

        public StatisticScoreModel GetStatisticScorebyId(int id, bool otherLang)
        {
            return GetStatisticScores(new StatisticScoreParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetStatisticScoreCount()
        {
            return _repository.StatisticCategory.Count();
        }
        #endregion

        #region MatchStatisticScore Services
        public IQueryable<MatchStatisticScoreModel> GetMatchStatisticScores(MatchStatisticScoreParameters parameters, bool otherLang)
        {
            return _repository.MatchStatisticScore
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new MatchStatisticScoreModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                          Fk_StatisticScore= a.Fk_StatisticScore,
                          Fk_TeamGameWeak  =a.Fk_TeamGameWeak,
                          IsCanNotEdit = a.IsCanNotEdit,
                          Value= a.Value,
                          ValuePercentage= a.ValuePercentage,
                          StatisticScore = new StatisticScoreModel
                          {
                              Id = a.Fk_StatisticScore,
                              Name = otherLang?a.StatisticScore.StatisticScoreLang.Name:a.StatisticScore.Name,
                              Fk_StatisticCategory = a.StatisticScore.Fk_StatisticCategory,
                              StatisticCategory = new StatisticCategoryModel
                              {
                                  Name = otherLang?a.StatisticScore.StatisticCategory.StatisticCategoryLang.Name:a.StatisticScore.StatisticCategory.Name
                              }
                          },
                          TeamGameWeak = new TeamGameWeakModel
                          {
                              Away = new TeamModel
                              {
                                  Name = otherLang ? a.TeamGameWeak.Away.TeamLang.Name : a.TeamGameWeak.Away.Name,
                                  ShortName = otherLang ? a.TeamGameWeak.Away.TeamLang.ShortName : a.TeamGameWeak.Away.ShortName,
                                  ImageUrl = a.TeamGameWeak.Away.StorageUrl + a.TeamGameWeak.Away.ImageUrl,
                                  ShirtImageUrl = a.TeamGameWeak.Away.ShirtStorageUrl + a.TeamGameWeak.Away.ShirtImageUrl,
                                  _365_TeamId = a.TeamGameWeak.Away._365_TeamId,
                              },
                              Home = new TeamModel
                              {
                                  Name = otherLang ? a.TeamGameWeak.Home.TeamLang.Name : a.TeamGameWeak.Home.Name,
                                  ShortName = otherLang ? a.TeamGameWeak.Home.TeamLang.ShortName : a.TeamGameWeak.Home.ShortName,
                                  ImageUrl = a.TeamGameWeak.Home.StorageUrl + a.TeamGameWeak.Home.ImageUrl,
                                  ShirtImageUrl = a.TeamGameWeak.Home.ShirtStorageUrl + a.TeamGameWeak.Home.ShirtImageUrl,
                                  _365_TeamId = a.TeamGameWeak.Home._365_TeamId
                              },
                              GameWeak = new GameWeakModel
                              {
                                  Name = otherLang ? a.TeamGameWeak.GameWeak.GameWeakLang.Name : a.TeamGameWeak.GameWeak.Name,
                                  _365_GameWeakId = a.TeamGameWeak.GameWeak._365_GameWeakId,
                                  Fk_Season = a.TeamGameWeak.GameWeak.Fk_Season,
                                  Season = new SeasonModel
                                  {
                                      Name = otherLang ? a.TeamGameWeak.GameWeak.Season.SeasonLang.Name : a.TeamGameWeak.GameWeak.Season.Name
                                  },
                              },
                              Fk_Away = a.TeamGameWeak.Fk_Away,
                              Fk_GameWeak = a.TeamGameWeak.Fk_GameWeak,
                              Fk_Home = a.TeamGameWeak.Fk_Home,
                              AwayScore = a.TeamGameWeak.AwayScore,
                              HomeScore = a.TeamGameWeak.HomeScore,
                              _365_MatchId = a.TeamGameWeak._365_MatchId,
                          }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<MatchStatisticScoreModel>> GetMatchStatisticScoresPaged(
                  MatchStatisticScoreParameters parameters, bool otherLang)
        {
            return await PagedList<MatchStatisticScoreModel>.ToPagedList(GetMatchStatisticScores(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<MatchStatisticScore> FindMatchStatisticScorebyId(int id, bool trackChanges)
        {
            return await _repository.MatchStatisticScore.FindById(id, trackChanges);
        }

        public void CreateMatchStatisticScore(MatchStatisticScore entiy)
        {
            _repository.MatchStatisticScore.Create(entiy);
        }

        public async Task DeleteMatchStatisticScore(int id)
        {
            MatchStatisticScore entity = await FindMatchStatisticScorebyId(id, trackChanges: true);
            _repository.MatchStatisticScore.Delete(entity);
        }

        public MatchStatisticScoreModel GetMatchStatisticScorebyId(int id, bool otherLang)
        {
            return GetMatchStatisticScores(new MatchStatisticScoreParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetMatchStatisticScoreCount()
        {
            return _repository.StatisticCategory.Count();
        }
        #endregion

    }
}
