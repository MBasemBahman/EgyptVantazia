using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerScoreModels;
using static Contracts.EnumData.DBModelsEnum;

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
                       .Select(x => new ScoreTypeModel
                       {
                           Id = x.Id,
                           CreatedAt = x.CreatedAt,
                           CreatedBy = x.CreatedBy,
                           LastModifiedAt = x.LastModifiedAt,
                           LastModifiedBy = x.LastModifiedBy,
                           Name = otherLang ? x.ScoreTypeLang.Name : x.Name,
                           _365_TypeId = x._365_TypeId,
                           Description = x.Description,
                           HavePoints = x.HavePoints,
                           IsEvent = x.IsEvent,
                           _365_EventTypeId = x._365_EventTypeId,
                           IsCanNotEdit = x.IsCanNotEdit
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

        public Dictionary<string, string> GetScoreTypesLookUp(ScoreTypeParameters parameters, bool otherLang)
        {
            return GetScoreTypes(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
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
            return GetScoreTypes(new ScoreTypeParameters { Id = id }, otherLang).FirstOrDefault();
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
                           Fk_TeamGameWeak = a.Fk_TeamGameWeak,
                           Fk_Player = a.Fk_Player,
                           Ranking = a.Ranking,
                           TotalPoints = a.TotalPoints,
                           _365_PlayerId = a._365_PlayerId,
                           TeamGameWeak = new TeamGameWeakModel
                           {
                               Fk_Away = a.TeamGameWeak.Fk_Away,
                               Fk_Home = a.TeamGameWeak.Fk_Home,
                               Fk_GameWeak = a.TeamGameWeak.Fk_GameWeak,
                               HomeScore = a.TeamGameWeak.HomeScore,
                               AwayScore = a.TeamGameWeak.AwayScore,
                               StartTime = a.TeamGameWeak.StartTime,
                               _365_MatchId = a.TeamGameWeak._365_MatchId,
                               IsEnded = a.TeamGameWeak.IsEnded,
                               Away = new TeamModel
                               {
                                   Name = otherLang ? a.TeamGameWeak.Away.TeamLang.Name : a.TeamGameWeak.Away.Name,
                                   ShortName = otherLang ? a.TeamGameWeak.Away.TeamLang.ShortName : a.TeamGameWeak.Away.ShortName,
                               },
                               Home = new TeamModel
                               {
                                   Name = otherLang ? a.TeamGameWeak.Home.TeamLang.Name : a.TeamGameWeak.Home.Name,
                                   ShortName = otherLang ? a.TeamGameWeak.Home.TeamLang.ShortName : a.TeamGameWeak.Home.ShortName,
                               },
                               GameWeak = new GameWeakModel
                               {
                                   Name = otherLang ? a.TeamGameWeak.GameWeak.GameWeakLang.Name : a.TeamGameWeak.GameWeak.Name,
                                   _365_GameWeakId = a.TeamGameWeak.GameWeak._365_GameWeakId,
                                   Season = new SeasonModel
                                   {
                                       Name = otherLang ? a.TeamGameWeak.GameWeak.Season.SeasonLang.Name : a.TeamGameWeak.GameWeak.Season.SeasonLang.Name,
                                   }
                               }
                           },
                           Player = new PlayerModel
                           {
                               Id = a.Fk_Player,
                               Name = otherLang ? a.Player.PlayerLang.Name : a.Player.Name,
                               ShortName = otherLang ? a.Player.PlayerLang.ShortName : a.Player.ShortName,
                               ImageUrl = !string.IsNullOrEmpty(a.Player.ImageUrl) ? a.Player.StorageUrl + a.Player.ImageUrl : a.Player.Team.ShirtStorageUrl + a.Player.Team.ShirtImageUrl,
                               _365_PlayerId = a.Player._365_PlayerId,
                               Fk_PlayerPosition = a.Player.Fk_PlayerPosition,
                               Fk_Team = a.Player.Fk_Team
                           },
                           PlayerGameWeakScores = parameters.IncludeScore ?
                                                  a.PlayerGameWeakScores
                                                   .Select(b => new PlayerGameWeakScoreModel
                                                   {
                                                       Id = b.Id,
                                                       Fk_ScoreType = b.Fk_ScoreType,
                                                       Points = b.Points,
                                                       Value = b.Value,
                                                       GameTime = b.GameTime,
                                                       FinalValue = b.FinalValue,
                                                       ScoreType = new ScoreTypeModel
                                                       {
                                                           Name = otherLang ? b.ScoreType.ScoreTypeLang.Name : b.ScoreType.Name,
                                                       },
                                                       IsOut = b.IsOut,
                                                       IsCanNotEdit = b.IsCanNotEdit
                                                   })
                                                   .ToList() : null
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
            return GetPlayerGameWeaks(new PlayerGameWeakParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetPlayerGameWeakCount()
        {
            return _repository.PlayerGameWeak.Count();
        }

        public PlayerGameWeak AddPlayerGameWeakScores(PlayerGameWeak player, List<PlayerGameWeakScoreCreateOrEditModel> scores)
        {
            if (scores != null && scores.Any())
            {
                foreach (PlayerGameWeakScoreCreateOrEditModel score in scores)
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
                foreach (PlayerGameWeakScoreCreateOrEditModel data in DataToUpdate)
                {
                    PlayerGameWeakScore dataDb = await FindPlayerGameWeakScorebyId(data.Id, trackChanges: true);
                    dataDb.Fk_ScoreType = data.Fk_ScoreType;
                    dataDb.Value = data.Value;
                    dataDb.Points = data.Points;
                }
            }
            return player;
        }

        public List<int> GetPlayerGameWeakTop(int fk_TeamGameWeak, int topCount)
        {
            return _repository.PlayerGameWeak.FindAll(new PlayerGameWeakParameters
            {
                Fk_TeamGameWeak = fk_TeamGameWeak,
            }, trackChanges: false).OrderByDescending(a => a.Ranking)
                                   .Skip(0)
                                   .Take(topCount)
                                   .Select(a => a.Fk_Player).ToList();
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
                           GameTime = a.GameTime,
                           FinalValue = a.FinalValue,
                           Fk_Team = a.PlayerGameWeak.Player.Fk_Team,
                           IsOut = a.IsOut,
                           IsCanNotEdit = a.IsCanNotEdit,
                           ScoreType = new ScoreTypeModel
                           {
                               Name = otherLang ? a.ScoreType.ScoreTypeLang.Name : a.ScoreType.Name,
                               Description = a.ScoreType.Description,
                               _365_TypeId = a.ScoreType._365_TypeId
                           },
                           PlayerGameWeak = new PlayerGameWeakModel
                           {
                               Player = new PlayerModel
                               {
                                   Id = a.PlayerGameWeak.Player.Id,
                                   Name = otherLang ? a.PlayerGameWeak.Player.PlayerLang.Name : a.PlayerGameWeak.Player.Name,
                                   ShortName = otherLang ? a.PlayerGameWeak.Player.PlayerLang.ShortName : a.PlayerGameWeak.Player.ShortName,
                               },
                               _365_PlayerId = a.PlayerGameWeak._365_PlayerId,
                               Ranking = a.PlayerGameWeak.Ranking,
                               Fk_Player = a.PlayerGameWeak.Fk_Player,
                           },
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
            if (PlayerGameWeakScore.Value == "0")
            {
                return;
            }

            if (PlayerGameWeakScore.Fk_ScoreType is
                    ((int)ScoreTypeEnum.Goal_Event) or
                    ((int)ScoreTypeEnum.Substitution_Event) or
                    ((int)ScoreTypeEnum.PenaltyKick_Event))
            {
                if (PlayerGameWeakScore.GameTime <= 0)
                {
                    return;
                }
            }

            if (PlayerGameWeakScore.Fk_ScoreType == (int)ScoreTypeEnum.Ranking)
            {
                if (PlayerGameWeakScore.Points <= 0)
                {
                    return;
                }
            }

            if (PlayerGameWeakScore.Fk_ScoreType == (int)ScoreTypeEnum.CleanSheet)
            {
                if (PlayerGameWeakScore.Points <= 0)
                {
                    return;
                }
            }
            if (PlayerGameWeakScore.Fk_ScoreType == (int)ScoreTypeEnum.ReceiveGoals)
            {
                if (PlayerGameWeakScore.Points >= 0)
                {
                    return;
                }
            }

            _repository.PlayerGameWeakScore.Create(PlayerGameWeakScore);
        }

        public async Task DeletePlayerGameWeakScore(int id)
        {
            PlayerGameWeakScore PlayerGameWeakScore = await FindPlayerGameWeakScorebyId(id, trackChanges: true);
            _repository.PlayerGameWeakScore.Delete(PlayerGameWeakScore);
        }

        public PlayerGameWeakScoreModel GetPlayerGameWeakScorebyId(int id, bool otherLang)
        {
            return GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetPlayerGameWeakScoreCount()
        {
            return _repository.PlayerGameWeakScore.Count();
        }

        public PlayerTotalScoreModel GetPlayerTotalScores(int fk_Player, int fk_Season, int fk_GameWeak, List<int> fk_ScoreTypes)
        {
            return _repository.PlayerGameWeakScore.GetPlayerTotalScores(fk_Player, fk_Season, fk_GameWeak, fk_ScoreTypes);
        }

        public void DeleteOldPlayerScores(int fk_PlayerGameWeak)
        {
            _repository.PlayerGameWeakScore.DeleteOldPlayerScores(fk_PlayerGameWeak);
        }
        #endregion
    }
}
