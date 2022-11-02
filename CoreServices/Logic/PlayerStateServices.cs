using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerStateModels;

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
                           Player = new PlayerModel
                           {
                               Id = a.Fk_Player,
                               Name = otherLang ? a.Player.PlayerLang.Name : a.Player.Name,
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
            return GetPlayerGameWeakScoreStates(new PlayerGameWeakScoreStateParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetPlayerGameWeakScoreStateCount()
        {
            return _repository.PlayerGameWeakScoreState.Count();
        }

        #endregion

        #region PlayerSeasonScoreState Services
        public IQueryable<PlayerSeasonScoreStateModel> GetPlayerSeasonScoreStates(PlayerSeasonScoreStateParameters parameters,
                bool otherLang)
        {
            return _repository.PlayerSeasonScoreState
                       .FindAll(parameters, trackChanges: false)
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
                           Player = new PlayerModel
                           {
                               Id = a.Fk_Player,
                               Name = otherLang ? a.Player.PlayerLang.Name : a.Player.Name,
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
            return GetPlayerSeasonScoreStates(new PlayerSeasonScoreStateParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetPlayerSeasonScoreStateCount()
        {
            return _repository.PlayerSeasonScoreState.Count();
        }

        #endregion

        #region ScoreState Services
        public IQueryable<ScoreStateModel> GetScoreStates(ScoreStateParameters parameters,
                bool otherLang)
        {
            return _repository.ScoreState
                       .FindAll(parameters, trackChanges: false)
                       .Select(b => new ScoreStateModel
                       {
                           Id = b.Id,
                           CreatedAt = b.CreatedAt,
                           CreatedBy = b.CreatedBy,
                           LastModifiedAt = b.LastModifiedAt,
                           LastModifiedBy = b.LastModifiedBy,
                           Name = otherLang ? b.ScoreStateLang.Name : b.Name,
                           Description = b.Description,
                           BestPlayer = parameters.IncludeBestPlayer == false ? null :
                                        parameters.Fk_Season > 0 ?
                                        b.PlayerSeasonScoreStates
                                         .OrderByDescending(a => a.Points)
                                         .Select(a => a.Player)
                                         .Select(a => new PlayerModel
                                         {
                                             Id = a.Id,
                                             Name = otherLang ? a.PlayerLang.Name : a.Name,
                                             ImageUrl = !string.IsNullOrEmpty(a.ImageUrl) ? a.StorageUrl + a.ImageUrl : a.Team.ShirtStorageUrl + a.Team.ShirtImageUrl,
                                             Fk_PlayerPosition = a.Fk_PlayerPosition,
                                             Fk_Team = a.Fk_Team,
                                             PlayerNumber = a.PlayerNumber,
                                             PlayerPosition = new PlayerPositionModel
                                             {
                                                 Name = otherLang ? a.PlayerPosition.PlayerPositionLang.Name : a.PlayerPosition.Name,
                                                 ImageUrl = a.PlayerPosition.StorageUrl + a.PlayerPosition.ImageUrl,
                                                 _365_PositionId = a.PlayerPosition._365_PositionId
                                             },
                                             Team = new TeamModel
                                             {
                                                 Name = otherLang ? a.Team.TeamLang.Name : a.Team.Name,
                                                 ImageUrl = a.Team.StorageUrl + a.Team.ImageUrl,
                                                 ShirtImageUrl = a.Team.ShirtStorageUrl + a.Team.ShirtImageUrl,
                                                 _365_TeamId = a.Team._365_TeamId
                                             },
                                             BuyPrice = a.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.BuyPrice).FirstOrDefault(),
                                             SellPrice = a.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.SellPrice).FirstOrDefault(),
                                             SeasonScoreStates = a.PlayerSeasonScoreStates
                                                .Where(c => c.Fk_Season == parameters.Fk_Season &&
                                                           b.Id == c.Fk_ScoreState)
                                                .Select(c => new PlayerSeasonScoreStateModel
                                                {
                                                    Id = c.Id,
                                                    Fk_Player = c.Fk_Player,
                                                    Points = c.Points,
                                                    Percent = c.Percent,
                                                    Value = c.Value,
                                                    Fk_ScoreState = c.Fk_ScoreState,
                                                    Fk_Season = c.Fk_Season,
                                                    LastModifiedAt = c.LastModifiedAt,
                                                })
                                                .ToList()
                                         })
                                         .FirstOrDefault() :
                                        parameters.Fk_GameWeak > 0 ?
                                        b.PlayerGameWeakScoreStates
                                         .OrderByDescending(a => a.Points)
                                         .Select(a => a.Player)
                                         .Select(a => new PlayerModel
                                         {
                                             Id = a.Id,
                                             Name = otherLang ? a.PlayerLang.Name : a.Name,
                                             ImageUrl = !string.IsNullOrEmpty(a.ImageUrl) ? a.StorageUrl + a.ImageUrl : a.Team.ShirtStorageUrl + a.Team.ShirtImageUrl,
                                             Fk_PlayerPosition = a.Fk_PlayerPosition,
                                             Fk_Team = a.Fk_Team,
                                             PlayerNumber = a.PlayerNumber,
                                             PlayerPosition = new PlayerPositionModel
                                             {
                                                 Name = otherLang ? a.PlayerPosition.PlayerPositionLang.Name : a.PlayerPosition.Name,
                                                 ImageUrl = a.PlayerPosition.StorageUrl + a.PlayerPosition.ImageUrl,
                                                 _365_PositionId = a.PlayerPosition._365_PositionId
                                             },
                                             Team = new TeamModel
                                             {
                                                 Name = otherLang ? a.Team.TeamLang.Name : a.Team.Name,
                                                 ImageUrl = a.Team.StorageUrl + a.Team.ImageUrl,
                                                 ShirtImageUrl = a.Team.ShirtStorageUrl + a.Team.ShirtImageUrl,
                                                 _365_TeamId = a.Team._365_TeamId
                                             },
                                             BuyPrice = a.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.BuyPrice).FirstOrDefault(),
                                             SellPrice = a.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.SellPrice).FirstOrDefault(),
                                             GameWeakScoreStates = a.PlayerGameWeakScoreStates
                                                .Where(c => c.Fk_GameWeak == parameters.Fk_GameWeak &&
                                                           b.Id == c.Fk_ScoreState)
                                                .Select(c => new PlayerGameWeakScoreStateModel
                                                {
                                                    Id = c.Id,
                                                    Fk_Player = c.Fk_Player,
                                                    Points = c.Points,
                                                    Percent = c.Percent,
                                                    Value = c.Value,
                                                    Fk_ScoreState = c.Fk_ScoreState,
                                                    Fk_GameWeak = c.Fk_GameWeak,
                                                    LastModifiedAt = c.LastModifiedAt,
                                                })
                                                .ToList()
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
            return GetScoreStates(new ScoreStateParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetScoreStateCount()
        {
            return _repository.ScoreState.Count();
        }
        #endregion

    }
}
