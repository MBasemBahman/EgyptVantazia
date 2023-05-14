using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerStateModels;
using static Contracts.EnumData.DBModelsEnum;

namespace CoreServices.Logic
{
    public class PlayerStateServices
    {
        private readonly RepositoryManager _repository;

        public PlayerStateServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region PlayerGameWeakScoreState Services
        public IQueryable<PlayerGameWeakScoreStateModel> GetPlayerGameWeakScoreStates(PlayerGameWeakScoreStateParameters parameters,
                bool otherLang)
        {
            return _repository.PlayerGameWeakScoreState
                       .FindAll(parameters, trackChanges: false)
                       .OrderByDescending(a => a.Points)
                       .ThenBy(a => a.Player.Id)
                       .Select(a => new PlayerGameWeakScoreStateModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Fk_Player = a.Fk_Player,
                           Percent = a.Percent,
                           Points = a.Points,
                           Value = a.Value,
                           Fk_ScoreState = a.Fk_ScoreState,
                           Fk_GameWeak = a.Fk_GameWeak,
                           Top15 = a.Top15,
                           Player = new PlayerModel
                           {
                               Id = a.Fk_Player,
                               Name = otherLang ? a.Player.PlayerLang.Name : a.Player.Name,
                               ShortName = otherLang ? a.Player.PlayerLang.ShortName : a.Player.ShortName,
                           },
                           GameWeak = new GameWeakModel
                           {
                               Id = a.Fk_GameWeak,
                               Name = otherLang ? a.GameWeak.GameWeakLang.Name : a.GameWeak.Name,
                           },
                           ScoreState = new ScoreStateModel
                           {
                               Id = a.Fk_ScoreState,
                               Name = otherLang ? a.ScoreState.ScoreStateLang.Name : a.ScoreState.Name,
                           },
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<PlayerGameWeakScoreStateModel>> GetPlayerGameWeakScoreStatePaged(
                  PlayerGameWeakScoreStateParameters parameters,
                  bool otherLang)
        {
            return await PagedList<PlayerGameWeakScoreStateModel>.ToPagedList(GetPlayerGameWeakScoreStates(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PlayerGameWeakScoreState> FindPlayerGameWeakScoreStatebyId(int id, bool trackChanges)
        {
            return await _repository.PlayerGameWeakScoreState.FindById(id, trackChanges);
        }

        public void ResetPlayerGameWeakScoreStateTop15(int fk_GameWeak)
        {
            _repository.PlayerGameWeakScoreState.ResetTop15(fk_GameWeak);
        }

        public void CreatePlayerGameWeakScoreState(PlayerGameWeakScoreState PlayerGameWeakScoreState)
        {
            _repository.PlayerGameWeakScoreState.Create(PlayerGameWeakScoreState);
        }

        public async Task DeletePlayerGameWeakScoreState(int id)
        {
            PlayerGameWeakScoreState PlayerGameWeakScoreState = await FindPlayerGameWeakScoreStatebyId(id, trackChanges: true);
            _repository.PlayerGameWeakScoreState.Delete(PlayerGameWeakScoreState);
        }

        public PlayerGameWeakScoreStateModel GetPlayerGameWeakScoreStatebyId(int id, bool otherLang)
        {
            return GetPlayerGameWeakScoreStates(new PlayerGameWeakScoreStateParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetPlayerGameWeakScoreStateCount()
        {
            return _repository.PlayerGameWeakScoreState.Count();
        }

        public void ResetGameWeakPlayerScores(int fk_Player, int fk_GameWeak)
        {
            _repository.PlayerGameWeakScoreState.DeleteOldPlayerScores(fk_Player, fk_GameWeak);
        }

        #endregion

        #region PlayerSeasonScoreState Services
        public IQueryable<PlayerSeasonScoreStateModel> GetPlayerSeasonScoreStates(PlayerSeasonScoreStateParameters parameters,
                bool otherLang)
        {
            return _repository.PlayerSeasonScoreState
                       .FindAll(parameters, trackChanges: false)
                       .OrderByDescending(a => a.Points)
                       .ThenBy(a => a.Player.Id)
                       .Select(a => new PlayerSeasonScoreStateModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Fk_Player = a.Fk_Player,
                           Points = a.Points,
                           Percent = a.Percent,
                           Value = a.Value,
                           Fk_ScoreState = a.Fk_ScoreState,
                           Fk_Season = a.Fk_Season,
                           Top15 = a.Top15,
                           Player = new PlayerModel
                           {
                               Id = a.Fk_Player,
                               Name = otherLang ? a.Player.PlayerLang.Name : a.Player.Name,
                               ShortName = otherLang ? a.Player.PlayerLang.ShortName : a.Player.ShortName,
                               ImageUrl = !string.IsNullOrEmpty(a.Player.ImageUrl) ? a.Player.StorageUrl + a.Player.ImageUrl : a.Player.Team.ShirtStorageUrl + a.Player.Team.ShirtImageUrl,
                           },
                           ScoreState = new ScoreStateModel
                           {
                               Id = a.Fk_ScoreState,
                               Name = otherLang ? a.ScoreState.ScoreStateLang.Name : a.ScoreState.Name,
                           },
                           Season = new SeasonModel
                           {
                               Id = a.Fk_Season,
                               Name = otherLang ? a.Season.SeasonLang.Name : a.Season.Name,
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<PlayerSeasonScoreStateModel>> GetPlayerSeasonScoreStatePaged(
                  PlayerSeasonScoreStateParameters parameters,
                  bool otherLang)
        {
            return await PagedList<PlayerSeasonScoreStateModel>.ToPagedList(GetPlayerSeasonScoreStates(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PlayerSeasonScoreState> FindPlayerSeasonScoreStatebyId(int id, bool trackChanges)
        {
            return await _repository.PlayerSeasonScoreState.FindById(id, trackChanges);
        }

        public void ResetPlayerSeasonScoreStateTop15(int fk_Season)
        {
            _repository.PlayerSeasonScoreState.ResetTop15(fk_Season);
        }

        public void CreatePlayerSeasonScoreState(PlayerSeasonScoreState PlayerSeasonScoreState)
        {
            _repository.PlayerSeasonScoreState.Create(PlayerSeasonScoreState);
        }

        public async Task DeletePlayerSeasonScoreState(int id)
        {
            PlayerSeasonScoreState PlayerSeasonScoreState = await FindPlayerSeasonScoreStatebyId(id, trackChanges: true);
            _repository.PlayerSeasonScoreState.Delete(PlayerSeasonScoreState);
        }

        public PlayerSeasonScoreStateModel GetPlayerSeasonScoreStatebyId(int id, bool otherLang)
        {
            return GetPlayerSeasonScoreStates(new PlayerSeasonScoreStateParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetPlayerSeasonScoreStateCount()
        {
            return _repository.PlayerSeasonScoreState.Count();
        }

        public void ResetSeasonPlayerScores(int fk_Player, int fk_Season)
        {
            _repository.PlayerSeasonScoreState.DeleteOldPlayerScores(fk_Player, fk_Season);
        }

        #endregion

        #region ScoreState Services
        public IQueryable<ScoreStateModel> GetScoreStates(ScoreStateParameters parameters,
                bool otherLang)
        {
            return _repository.ScoreState
                       .FindAll(parameters, trackChanges: false)
                       .Select(scoreState => new ScoreStateModel
                       {
                           Id = scoreState.Id,
                           CreatedAt = scoreState.CreatedAt,
                           CreatedBy = scoreState.CreatedBy,
                           LastModifiedAt = scoreState.LastModifiedAt,
                           LastModifiedBy = scoreState.LastModifiedBy,
                           Name = otherLang ? scoreState.ScoreStateLang.Name : scoreState.Name,
                           Description = scoreState.Description,
                           BestPlayer = parameters.IncludeBestPlayer == false ? null :
                                        parameters.Fk_Season > 0 ?
                                        scoreState.PlayerSeasonScoreStates
                                         .Where(a => scoreState.Id != (int)ScoreStateEnum.CleanSheet ||
                                                     a.Player.Fk_PlayerPosition == (int)PlayerPositionEnum.Goalkeeper)
                                         .OrderByDescending(a => a.Points)
                                         .ThenBy(a => a.Player.Id)
                                         .Select(playerScore => new PlayerModel
                                         {
                                             Id = playerScore.Player.Id,
                                             Name = otherLang ? playerScore.Player.PlayerLang.Name : playerScore.Player.Name,
                                             ShortName = otherLang ? playerScore.Player.PlayerLang.ShortName : playerScore.Player.ShortName,
                                             ImageUrl = !string.IsNullOrEmpty(playerScore.Player.ImageUrl) ? playerScore.Player.StorageUrl + playerScore.Player.ImageUrl : playerScore.Player.Team.ShirtStorageUrl + playerScore.Player.Team.ShirtImageUrl,
                                             Fk_PlayerPosition = playerScore.Player.Fk_PlayerPosition,
                                             Fk_Team = playerScore.Player.Fk_Team,
                                             PlayerNumber = playerScore.Player.PlayerNumber,
                                             PlayerPosition = new PlayerPositionModel
                                             {
                                                 Name = otherLang ? playerScore.Player.PlayerPosition.PlayerPositionLang.Name : playerScore.Player.PlayerPosition.Name,
                                                 ShortName = otherLang ? playerScore.Player.PlayerPosition.PlayerPositionLang.ShortName : playerScore.Player.PlayerPosition.ShortName,
                                                 ImageUrl = playerScore.Player.PlayerPosition.StorageUrl + playerScore.Player.PlayerPosition.ImageUrl,
                                                 _365_PositionId = playerScore.Player.PlayerPosition._365_PositionId
                                             },
                                             Team = new TeamModel
                                             {
                                                 Name = otherLang ? playerScore.Player.Team.TeamLang.Name : playerScore.Player.Team.Name,
                                                 ShortName = otherLang ? playerScore.Player.Team.TeamLang.ShortName : playerScore.Player.Team.ShortName,
                                                 ImageUrl = playerScore.Player.Team.StorageUrl + playerScore.Player.Team.ImageUrl,
                                                 ShirtImageUrl = playerScore.Player.Team.ShirtStorageUrl + playerScore.Player.Team.ShirtImageUrl,
                                                 _365_TeamId = playerScore.Player.Team._365_TeamId
                                             },
                                             BuyPrice = playerScore.Player.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.BuyPrice).FirstOrDefault(),
                                             SellPrice = playerScore.Player.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.SellPrice).FirstOrDefault(),
                                             SeasonScoreStates = new List<PlayerSeasonScoreStateModel>
                                             {
                                                new PlayerSeasonScoreStateModel
                                                {
                                                    Points = playerScore.Points,
                                                    Percent = playerScore.Percent,
                                                    Value = playerScore.Value,
                                                    LastModifiedAt = playerScore.LastModifiedAt,
                                                }
                                             }
                                         })
                                         .FirstOrDefault() :
                                        parameters.Fk_GameWeak > 0 ?
                                        scoreState.PlayerGameWeakScoreStates
                                         .Where(a => scoreState.Id != (int)ScoreStateEnum.CleanSheet ||
                                                     a.Player.Fk_PlayerPosition == (int)PlayerPositionEnum.Goalkeeper)
                                         .OrderByDescending(a => a.Points)
                                         .ThenBy(a => a.Player.Id)
                                         .Select(playerScore => new PlayerModel
                                         {
                                             Id = playerScore.Player.Id,
                                             Name = otherLang ? playerScore.Player.PlayerLang.Name : playerScore.Player.Name,
                                             ShortName = otherLang ? playerScore.Player.PlayerLang.ShortName : playerScore.Player.ShortName,
                                             ImageUrl = !string.IsNullOrEmpty(playerScore.Player.ImageUrl) ? playerScore.Player.StorageUrl + playerScore.Player.ImageUrl : playerScore.Player.Team.ShirtStorageUrl + playerScore.Player.Team.ShirtImageUrl,
                                             Fk_PlayerPosition = playerScore.Player.Fk_PlayerPosition,
                                             Fk_Team = playerScore.Player.Fk_Team,
                                             PlayerNumber = playerScore.Player.PlayerNumber,
                                             PlayerPosition = new PlayerPositionModel
                                             {
                                                 Name = otherLang ? playerScore.Player.PlayerPosition.PlayerPositionLang.Name : playerScore.Player.PlayerPosition.Name,
                                                 ShortName = otherLang ? playerScore.Player.PlayerPosition.PlayerPositionLang.ShortName : playerScore.Player.PlayerPosition.ShortName,
                                                 ImageUrl = playerScore.Player.PlayerPosition.StorageUrl + playerScore.Player.PlayerPosition.ImageUrl,
                                                 _365_PositionId = playerScore.Player.PlayerPosition._365_PositionId
                                             },
                                             Team = new TeamModel
                                             {
                                                 Name = otherLang ? playerScore.Player.Team.TeamLang.Name : playerScore.Player.Team.Name,
                                                 ShortName = otherLang ? playerScore.Player.Team.TeamLang.ShortName : playerScore.Player.Team.ShortName,
                                                 ImageUrl = playerScore.Player.Team.StorageUrl + playerScore.Player.Team.ImageUrl,
                                                 ShirtImageUrl = playerScore.Player.Team.ShirtStorageUrl + playerScore.Player.Team.ShirtImageUrl,
                                                 _365_TeamId = playerScore.Player.Team._365_TeamId
                                             },
                                             BuyPrice = playerScore.Player.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.BuyPrice).FirstOrDefault(),
                                             SellPrice = playerScore.Player.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.SellPrice).FirstOrDefault(),
                                             GameWeakScoreStates = new List<PlayerGameWeakScoreStateModel>
                                             {
                                                new PlayerGameWeakScoreStateModel
                                                {
                                                    Points = playerScore.Points,
                                                    Percent = playerScore.Percent,
                                                    Value = playerScore.Value,
                                                    LastModifiedAt = playerScore.LastModifiedAt,
                                                }
                                             }
                                         })
                                         .FirstOrDefault() : null
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<ScoreStateModel>> GetScoreStatePaged(
                  ScoreStateParameters parameters,
                  bool otherLang)
        {
            return await PagedList<ScoreStateModel>.ToPagedList(GetScoreStates(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<ScoreState> FindScoreStatebyId(int id, bool trackChanges)
        {
            return await _repository.ScoreState.FindById(id, trackChanges);
        }

        public Dictionary<string, string> GetScoreStatesLookUp(ScoreStateParameters parameters, bool otherLang)
        {
            return GetScoreStates(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }
        public void CreateScoreState(ScoreState ScoreState)
        {
            _repository.ScoreState.Create(ScoreState);
        }

        public async Task DeleteScoreState(int id)
        {
            ScoreState ScoreState = await FindScoreStatebyId(id, trackChanges: true);
            _repository.ScoreState.Delete(ScoreState);
        }

        public ScoreStateModel GetScoreStatebyId(int id, bool otherLang)
        {
            return GetScoreStates(new ScoreStateParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetScoreStateCount()
        {
            return _repository.ScoreState.Count();
        }
        #endregion

    }
}
