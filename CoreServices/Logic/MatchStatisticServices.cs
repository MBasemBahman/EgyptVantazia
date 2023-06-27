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

        public IQueryable<StatisticCategory> GetStatisticCategorys(StatisticCategoryParameters parameters)
        {
            return _repository.StatisticCategory
                       .FindAll(parameters, trackChanges: false);
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
                           StatisticCategory = new StatisticCategoryModel
                           {
                               Name = otherLang ? a.StatisticCategory.StatisticCategoryLang.Name : a.StatisticCategory.Name
                           },
                           _365_Id = a._365_Id
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public IQueryable<StatisticScore> GetStatisticScores(StatisticScoreParameters parameters)
        {
            return _repository.StatisticScore
                       .FindAll(parameters, trackChanges: false);
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
                           Fk_StatisticScore = a.Fk_StatisticScore,
                           Fk_TeamGameWeak = a.Fk_TeamGameWeak,
                           IsCanNotEdit = a.IsCanNotEdit,
                           Value = a.Value,
                           ValuePercentage = a.ValuePercentage,
                           Fk_Team = a.Fk_Team,
                           Team = new TeamModel
                           {
                               Id = a.Team.Id,
                               Name = a.Team.Name,
                               ShortName = a.Team.ShortName,
                               _365_TeamId = a.Team._365_TeamId,
                               ShirtImageUrl = a.Team.ShirtStorageUrl + a.Team.ShirtImageUrl,
                               ImageUrl = a.Team.StorageUrl + a.Team.ImageUrl,
                           },
                           StatisticScore = new StatisticScoreModel
                           {
                               Id = a.Fk_StatisticScore,
                               Name = otherLang ? a.StatisticScore.StatisticScoreLang.Name : a.StatisticScore.Name,
                               Fk_StatisticCategory = a.StatisticScore.Fk_StatisticCategory,
                               StatisticCategory = new StatisticCategoryModel
                               {
                                   Name = otherLang ? a.StatisticScore.StatisticCategory.StatisticCategoryLang.Name : a.StatisticScore.StatisticCategory.Name
                               }
                           },
                           TeamGameWeak = new TeamGameWeakModel
                           {
                               GameWeak = new GameWeakModel
                               {
                                   Name = otherLang ? a.TeamGameWeak.GameWeak.GameWeakLang.Name : a.TeamGameWeak.GameWeak.Name,
                                   _365_GameWeakId = a.TeamGameWeak.GameWeak._365_GameWeakId,
                                   Fk_Season = a.TeamGameWeak.GameWeak.Fk_Season,
                                   Season = new SeasonModel
                                   {
                                       Name = otherLang ? a.TeamGameWeak.GameWeak.Season.SeasonLang.Name : a.TeamGameWeak.GameWeak.Season.Name
                                   },
                               }
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public IQueryable<MatchStatisticScore> GetMatchStatisticScores(MatchStatisticScoreParameters parameters)
        {
            return _repository.MatchStatisticScore
                       .FindAll(parameters, trackChanges: false);
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
