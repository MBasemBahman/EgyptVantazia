using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.PlayerStateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                           PositionByPercent = a.PositionByPercent,
                           PositionByPoints = a.PositionByPoints,
                           PositionByValue = a.PositionByValue,
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
                           PositionByPercent = a.PositionByPercent,
                           PositionByPoints = a.PositionByPoints,
                           PositionByValue = a.PositionByValue,
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
                       .Select(a => new ScoreStateModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Name = otherLang ? a.ScoreStateLang.Name : a.Name,
                           Description = a.Description,
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
