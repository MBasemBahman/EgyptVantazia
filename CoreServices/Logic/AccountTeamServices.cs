using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountTeamModels;

namespace CoreServices.Logic
{
    public class AccountTeamServices
    {
        private readonly RepositoryManager _repository;

        public AccountTeamServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region AccountTeam Services
        public IQueryable<AccountTeamModel> GetAccountTeams(AccountTeamParameters parameters,
                bool otherLang)
        {
            return _repository.AccountTeam
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new AccountTeamModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Fk_Account = a.Fk_Account,
                           Fk_Season = a.Fk_Season,
                           Name = a.Name,
                           TotalMoney = a.TotalMoney,
                           TotalPoints = a.TotalPoints,
                           ImageUrl = a.StorageUrl + a.ImageUrl,
                           Season = new SeasonModel
                           {
                               Name = otherLang ? a.Season.SeasonLang.Name : a.Season.Name,
                               ImageUrl = a.Season.StorageUrl + a.Season.ImageUrl,
                               _365_SeasonId = a.Season._365_SeasonId
                           },
                           Account = new AccountModel
                           {
                               ImageUrl = a.Account.StorageUrl + a.Account.ImageUrl,
                               FullName = a.Account.FullName,
                           },
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<AccountTeamModel>> GetAccountTeamPaged(
                  AccountTeamParameters parameters,
                  bool otherLang)
        {
            return await PagedList<AccountTeamModel>.ToPagedList(GetAccountTeams(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<AccountTeam> FindAccountTeambyId(int id, bool trackChanges)
        {
            return await _repository.AccountTeam.FindById(id, trackChanges);
        }

        public void CreateAccountTeam(AccountTeam AccountTeam)
        {
            _repository.AccountTeam.Create(AccountTeam);
        }

        public async Task DeleteAccountTeam(int id)
        {
            AccountTeam AccountTeam = await FindAccountTeambyId(id, trackChanges: true);
            _repository.AccountTeam.Delete(AccountTeam);
        }

        public AccountTeamModel GetAccountTeambyId(int id, bool otherLang)
        {
            return GetAccountTeams(new AccountTeamParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetAccountTeamCount()
        {
            return _repository.AccountTeam.Count();
        }

        public AccountTeamModel GetCurrentTeam(int fk_Account, int fk_Season)
        {
            return GetAccountTeams(new AccountTeamParameters()
            {
                Fk_Account = fk_Account,
                Fk_Season = fk_Season
            }, otherLang: false).FirstOrDefault();
        }

        public async Task<string> UploadAccountTeamImage(string rootPath, IFormFile file)
        {
            FileUploader uploader = new(rootPath);
            return await uploader.UploudFile(file, "Uploud/AccountTeam");
        }

        #endregion

        #region AccountTeamGameWeak Services
        public IQueryable<AccountTeamGameWeakModel> GetAccountTeamGameWeaks(AccountTeamGameWeakParameters parameters,
                bool otherLang)
        {
            return _repository.AccountTeamGameWeak
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new AccountTeamGameWeakModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           FreeHit = a.FreeHit,
                           TotalPoints = a.TotalPoints,
                           WildCard = a.WildCard,
                           Fk_AccountTeam = a.Fk_AccountTeam,
                           Fk_GameWeak = a.Fk_GameWeak,
                           BenchBoost = a.BenchBoost,
                           GameWeak = new GameWeakModel
                           {
                               Name = otherLang ? a.GameWeak.GameWeakLang.Name : a.GameWeak.Name,
                               _365_GameWeakId = a.GameWeak._365_GameWeakId,
                               Fk_Season = a.GameWeak.Fk_Season,
                               Season = new SeasonModel
                               {
                                   Name = otherLang ? a.GameWeak.Season.SeasonLang.Name : a.GameWeak.Season.Name
                               }
                           },
                           AccountTeam = new AccountTeamModel
                           {
                               Name = a.AccountTeam.Name
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<AccountTeamGameWeakModel>> GetAccountTeamGameWeakPaged(
                  AccountTeamGameWeakParameters parameters,
                  bool otherLang)
        {
            return await PagedList<AccountTeamGameWeakModel>.ToPagedList(GetAccountTeamGameWeaks(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<AccountTeamGameWeak> FindAccountTeamGameWeakbyId(int id, bool trackChanges)
        {
            return await _repository.AccountTeamGameWeak.FindById(id, trackChanges);
        }

        public void CreateAccountTeamGameWeak(AccountTeamGameWeak AccountTeamGameWeak)
        {
            _repository.AccountTeamGameWeak.Create(AccountTeamGameWeak);
        }

        public async Task DeleteAccountTeamGameWeak(int id)
        {
            AccountTeamGameWeak AccountTeamGameWeak = await FindAccountTeamGameWeakbyId(id, trackChanges: true);
            _repository.AccountTeamGameWeak.Delete(AccountTeamGameWeak);
        }

        public AccountTeamGameWeakModel GetAccountTeamGameWeakbyId(int id, bool otherLang)
        {
            return GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetAccountTeamGameWeakCount()
        {
            return _repository.AccountTeamGameWeak.Count();
        }
        #endregion

        #region AccountTeamPlayerGameWeak Services
        public IQueryable<AccountTeamPlayerGameWeakModel> GetAccountTeamPlayerGameWeaks(AccountTeamPlayerGameWeakParameters parameters,
                bool otherLang)
        {
            return _repository.AccountTeamPlayerGameWeak
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new AccountTeamPlayerGameWeakModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Fk_AccountTeamPlayer = a.Fk_AccountTeamPlayer,
                           Fk_GameWeak = a.Fk_GameWeak,
                           Fk_TeamPlayerType = a.Fk_TeamPlayerType,
                           TrippleCaptain = a.TrippleCaptain,
                           GameWeak = new GameWeakModel
                           {
                               Name = otherLang ? a.GameWeak.GameWeakLang.Name : a.GameWeak.Name,
                               _365_GameWeakId = a.GameWeak._365_GameWeakId,
                               Fk_Season = a.GameWeak.Fk_Season,
                               Season = new SeasonModel
                               {
                                   Name = otherLang ? a.GameWeak.Season.SeasonLang.Name : a.GameWeak.Season.Name
                               }
                           },
                           TeamPlayerType = new TeamPlayerTypeModel
                           {
                               Name = otherLang ? a.TeamPlayerType.TeamPlayerTypeLang.Name : a.TeamPlayerType.Name,
                           },
                           AccountTeamPlayer = new AccountTeamPlayerModel
                           {
                               Player = new PlayerModel
                               {
                                   Name = otherLang ? a.AccountTeamPlayer.Player.PlayerLang.Name : a.AccountTeamPlayer.Player.Name,
                                   ImageUrl = a.AccountTeamPlayer.Player.StorageUrl + a.AccountTeamPlayer.Player.ImageUrl,
                                   _365_PlayerId = a.AccountTeamPlayer.Player._365_PlayerId
                               },
                               AccountTeam = new AccountTeamModel
                               {
                                   Name = a.AccountTeamPlayer.AccountTeam.Name
                               }
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<AccountTeamPlayerGameWeakModel>> GetAccountTeamPlayerGameWeakPaged(
                  AccountTeamPlayerGameWeakParameters parameters,
                  bool otherLang)
        {
            return await PagedList<AccountTeamPlayerGameWeakModel>.ToPagedList(GetAccountTeamPlayerGameWeaks(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<AccountTeamPlayerGameWeak> FindAccountTeamPlayerGameWeakbyId(int id, bool trackChanges)
        {
            return await _repository.AccountTeamPlayerGameWeak.FindById(id, trackChanges);
        }

        public void CreateAccountTeamPlayerGameWeak(AccountTeamPlayerGameWeak AccountTeamPlayerGameWeak)
        {
            _repository.AccountTeamPlayerGameWeak.Create(AccountTeamPlayerGameWeak);
        }

        public async Task DeleteAccountTeamPlayerGameWeak(int id)
        {
            AccountTeamPlayerGameWeak AccountTeamPlayerGameWeak = await FindAccountTeamPlayerGameWeakbyId(id, trackChanges: true);
            _repository.AccountTeamPlayerGameWeak.Delete(AccountTeamPlayerGameWeak);
        }

        public AccountTeamPlayerGameWeakModel GetAccountTeamPlayerGameWeakbyId(int id, bool otherLang)
        {
            return GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetAccountTeamPlayerGameWeakCount()
        {
            return _repository.AccountTeamPlayerGameWeak.Count();
        }
        #endregion

        #region AccountTeamPlayer Services
        public IQueryable<AccountTeamPlayerModel> GetAccountTeamPlayers(AccountTeamPlayerParameters parameters,
                bool otherLang)
        {
            return _repository.AccountTeamPlayer
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new AccountTeamPlayerModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           Fk_AccountTeam = a.Fk_AccountTeam,
                           Fk_Player = a.Fk_Player,
                           Player = new PlayerModel
                           {
                               Name = otherLang ? a.Player.PlayerLang.Name : a.Player.Name,
                               ImageUrl = a.Player.StorageUrl + a.Player.ImageUrl,
                               _365_PlayerId = a.Player._365_PlayerId,
                               Fk_Team = a.Player.Fk_Team,
                               Team = new TeamModel
                               {
                                   Name =otherLang?a.Player.Team.TeamLang.Name : a.Player.Team.Name
                               }
                           },
                           AccountTeam = new AccountTeamModel
                           {
                               Name = a.AccountTeam.Name
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<AccountTeamPlayerModel>> GetAccountTeamPlayerPaged(
                  AccountTeamPlayerParameters parameters,
                  bool otherLang)
        {
            return await PagedList<AccountTeamPlayerModel>.ToPagedList(GetAccountTeamPlayers(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<AccountTeamPlayer> FindAccountTeamPlayerbyId(int id, bool trackChanges)
        {
            return await _repository.AccountTeamPlayer.FindById(id, trackChanges);
        }

        public void CreateAccountTeamPlayer(AccountTeamPlayer AccountTeamPlayer)
        {
            _repository.AccountTeamPlayer.Create(AccountTeamPlayer);
        }

        public async Task DeleteAccountTeamPlayer(int id)
        {
            AccountTeamPlayer AccountTeamPlayer = await FindAccountTeamPlayerbyId(id, trackChanges: true);
            _repository.AccountTeamPlayer.Delete(AccountTeamPlayer);
        }

        public AccountTeamPlayerModel GetAccountTeamPlayerbyId(int id, bool otherLang)
        {
            return GetAccountTeamPlayers(new AccountTeamPlayerParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetAccountTeamPlayerCount()
        {
            return _repository.AccountTeamPlayer.Count();
        }
        #endregion

        #region TeamPlayerType Services
        public IQueryable<TeamPlayerTypeModel> GetTeamPlayerTypes(RequestParameters parameters,
                bool otherLang)
        {
            return _repository.TeamPlayerType
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new TeamPlayerTypeModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Name = otherLang ? a.TeamPlayerTypeLang.Name : a.Name,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<TeamPlayerTypeModel>> GetTeamPlayerTypePaged(
                  RequestParameters parameters,
                  bool otherLang)
        {
            return await PagedList<TeamPlayerTypeModel>.ToPagedList(GetTeamPlayerTypes(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<TeamPlayerType> FindTeamPlayerTypebyId(int id, bool trackChanges)
        {
            return await _repository.TeamPlayerType.FindById(id, trackChanges);
        }

        public void CreateTeamPlayerType(TeamPlayerType TeamPlayerType)
        {
            _repository.TeamPlayerType.Create(TeamPlayerType);
        }

        public async Task DeleteTeamPlayerType(int id)
        {
            TeamPlayerType TeamPlayerType = await FindTeamPlayerTypebyId(id, trackChanges: true);
            _repository.TeamPlayerType.Delete(TeamPlayerType);
        }

        public TeamPlayerTypeModel GetTeamPlayerTypebyId(int id, bool otherLang)
        {
            return GetTeamPlayerTypes(new RequestParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetTeamPlayerTypeCount()
        {
            return _repository.TeamPlayerType.Count();
        }
        #endregion
    }
}
