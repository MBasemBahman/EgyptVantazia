using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.TeamModels;

namespace CoreServices.Logic
{
    public class PlayerScoreServices
    {
        private readonly RepositoryManager _repository;

        public PlayerScoreServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region ScoreType Services
        public IQueryable<ScoreTypeModel> GetScoreTypes(ScoreTypeParameters parameters,
                bool otherLang)
        {
            return _repository.ScoreType
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new ScoreTypeModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Name = otherLang ? a.ScoreTypeLang.Name : a.Name,
                           _365_TypeId = a._365_TypeId
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<ScoreTypeModel>> GetScoreTypePaged(
                  ScoreTypeParameters parameters,
                  bool otherLang)
        {
            return await PagedList<ScoreTypeModel>.ToPagedList(GetScoreTypes(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<ScoreType> FindScoreTypebyId(int id, bool trackChanges)
        {
            return await _repository.ScoreType.FindById(id, trackChanges);
        }

        public void CreateScoreType(ScoreType ScoreType)
        {
            _repository.ScoreType.Create(ScoreType);
        }

        public async Task DeleteScoreType(int id)
        {
            ScoreType ScoreType = await FindScoreTypebyId(id, trackChanges: true);
            _repository.ScoreType.Delete(ScoreType);
        }

        public ScoreTypeModel GetScoreTypebyId(int id, bool otherLang)
        {
            return GetScoreTypes(new ScoreTypeParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetScoreTypeCount()
        {
            return _repository.ScoreType.Count();
        }
        #endregion

        #region PlayerGameWeak Services
        public IQueryable<PlayerGameWeakModel> GetPlayerGameWeaks(PlayerGameWeakParameters parameters,
                bool otherLang)
        {
            return _repository.PlayerGameWeak
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new PlayerGameWeakModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Fk_GameWeak = a.Fk_GameWeak,
                           Fk_Player = a.Fk_Player,
                           GameWeak = new GameWeakModel
                           {
                               Name = otherLang ? a.GameWeak.GameWeakLang.Name : a.GameWeak.Name,
                               _365_GameWeakId = a.GameWeak._365_GameWeakId
                           },
                           Player = new PlayerModel
                           {
                               Name = otherLang ? a.Player.PlayerLang.Name : a.Player.Name,
                               ImageUrl = a.Player.StorageUrl + a.Player.ImageUrl,
                               _365_PlayerId = a.Player._365_PlayerId
                           },
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<PlayerGameWeakModel>> GetPlayerGameWeakPaged(
                  PlayerGameWeakParameters parameters,
                  bool otherLang)
        {
            return await PagedList<PlayerGameWeakModel>.ToPagedList(GetPlayerGameWeaks(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PlayerGameWeak> FindPlayerGameWeakbyId(int id, bool trackChanges)
        {
            return await _repository.PlayerGameWeak.FindById(id, trackChanges);
        }

        public void CreatePlayerGameWeak(PlayerGameWeak PlayerGameWeak)
        {
            _repository.PlayerGameWeak.Create(PlayerGameWeak);
        }

        public async Task DeletePlayerGameWeak(int id)
        {
            PlayerGameWeak PlayerGameWeak = await FindPlayerGameWeakbyId(id, trackChanges: true);
            _repository.PlayerGameWeak.Delete(PlayerGameWeak);
        }

        public PlayerGameWeakModel GetPlayerGameWeakbyId(int id, bool otherLang)
        {
            return GetPlayerGameWeaks(new PlayerGameWeakParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetPlayerGameWeakCount()
        {
            return _repository.PlayerGameWeak.Count();
        }

        public PlayerGameWeak AddPlayerGameWeakScores(PlayerGameWeak player, List<PlayerGameWeakScoreCreateOrEditModel> scores)
        {

            if (scores != null && scores.Any())
            {
                foreach (var score in scores)
                {
                    CreatePlayerGameWeakScore(new PlayerGameWeakScore
                    {
                        Fk_PlayerGameWeak = player.Id,
                        Fk_ScoreType = score.Fk_ScoreType,
                        Value = score.Value,
                        Points = score.Points
                    });
                }
            }
            return player;
        }

        public async Task<PlayerGameWeak> DeletePlayerGameWeakScores(PlayerGameWeak player, List<int> scoreIds)
        {
            if (scoreIds != null && scoreIds.Any())
            {
                foreach (int id in scoreIds)
                {
                    await DeletePlayerGameWeakScore(id);
                }
            }

            return player;
        }

        public async Task<PlayerGameWeak> UpdatePlayerGameWeakScores(PlayerGameWeak player, List<PlayerGameWeakScoreCreateOrEditModel> newData)
        {
            List<int> oldData = GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters { Fk_PlayerGameWeak = player.Id }, otherLang: false).Select(a => a.Id).ToList();

            List<int> AddData = newData.Select(a => a.Id).ToList().Except(oldData).ToList();

            List<int> RmvData = oldData.Except(newData.Select(a => a.Id).ToList()).ToList();

            List<PlayerGameWeakScoreCreateOrEditModel> DataToUpdate = newData.Where(a => oldData.Contains(a.Id)).ToList();

            player = AddPlayerGameWeakScores(player, newData.Where(a => AddData.Contains(a.Id)).ToList());

            player = await DeletePlayerGameWeakScores(player, RmvData);

            if (DataToUpdate != null && DataToUpdate.Any())
            {
                foreach (var data in DataToUpdate)
                {
                    PlayerGameWeakScore dataDb = await FindPlayerGameWeakScorebyId(data.Id, trackChanges: true);
                    dataDb.Fk_ScoreType = data.Fk_ScoreType;
                    dataDb.Value = data.Value;
                    dataDb.Points = data.Points;
                }
            }
            return player;
        }
        #endregion

        #region PlayerGameWeakScore Services
        public IQueryable<PlayerGameWeakScoreModel> GetPlayerGameWeakScores(PlayerGameWeakScoreParameters parameters,
                bool otherLang)
        {
            return _repository.PlayerGameWeakScore
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new PlayerGameWeakScoreModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Fk_PlayerGameWeak = a.Fk_PlayerGameWeak,
                           Fk_ScoreType = a.Fk_ScoreType,
                           Points = a.Points,
                           Value = a.Value,
                           ScoreType = new ScoreTypeModel
                           {
                               Name = otherLang ? a.ScoreType.ScoreTypeLang.Name : a.ScoreType.Name,
                               _365_TypeId = a.ScoreType._365_TypeId
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<PlayerGameWeakScoreModel>> GetPlayerGameWeakScorePaged(
                  PlayerGameWeakScoreParameters parameters,
                  bool otherLang)
        {
            return await PagedList<PlayerGameWeakScoreModel>.ToPagedList(GetPlayerGameWeakScores(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PlayerGameWeakScore> FindPlayerGameWeakScorebyId(int id, bool trackChanges)
        {
            return await _repository.PlayerGameWeakScore.FindById(id, trackChanges);
        }

        public void CreatePlayerGameWeakScore(PlayerGameWeakScore PlayerGameWeakScore)
        {
            _repository.PlayerGameWeakScore.Create(PlayerGameWeakScore);
        }

        public async Task DeletePlayerGameWeakScore(int id)
        {
            PlayerGameWeakScore PlayerGameWeakScore = await FindPlayerGameWeakScorebyId(id, trackChanges: true);
            _repository.PlayerGameWeakScore.Delete(PlayerGameWeakScore);
        }

        public PlayerGameWeakScoreModel GetPlayerGameWeakScorebyId(int id, bool otherLang)
        {
            return GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetPlayerGameWeakScoreCount()
        {
            return _repository.PlayerGameWeakScore.Count();
        }
        #endregion
    }
}
