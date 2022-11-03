using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountTeamModels;
using static Contracts.EnumData.DBModelsEnum;

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
                           CountryRanking = a.CountryRanking,
                           GlobalRanking = a.GlobalRanking,
                           FavouriteTeamRanking = a.FavouriteTeamRanking,
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
                           PlayersCount = a.AccountTeamPlayers.Count,
                           FreeTransfer = a.FreeTransfer
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
                           IsIncreasing = a.TotalPoints > 0,
                           FreeHit = a.FreeHit,
                           TotalPoints = a.TotalPoints,
                           WildCard = a.WildCard,
                           Fk_AccountTeam = a.Fk_AccountTeam,
                           Fk_GameWeak = a.Fk_GameWeak,
                           BenchBoost = a.BenchBoost,
                           DoubleGameWeak = a.DoubleGameWeak,
                           TansfarePoints = a.TansfarePoints,
                           TansfareCount = a.AccountTeam.PlayerTransfers.Count(b => b.Fk_GameWeak == parameters.Fk_GameWeak),
                           Top_11 = a.Top_11,
                           GlobalRanking = a.GlobalRanking,
                           CountryRanking = a.CountryRanking,
                           FavouriteTeamRanking = a.FavouriteTeamRanking,
                           AvailableBenchBoost = !a.BenchBoost,
                           AvailableDoubleGameWeak = !a.DoubleGameWeak,
                           AvailableFreeHit = !a.FreeHit,
                           AvailableTop_11 = !a.Top_11,
                           AvailableWildCard = !a.WildCard,
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

        public AccountTeamGameWeakModel GetTeamGameWeak(int fk_Account, int fk_GameWeak)
        {
            return GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters()
            {
                Fk_Account = fk_Account,
                Fk_GameWeak = fk_GameWeak
            }, otherLang: false).FirstOrDefault();
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
                           IsPrimary = a.IsPrimary,
                           IsTransfer = a.IsTransfer,
                           Order = a.Order,
                           Points = a.Points,
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
                                   ImageUrl = !string.IsNullOrEmpty(a.AccountTeamPlayer.Player.ImageUrl) ? a.AccountTeamPlayer.Player.StorageUrl + a.AccountTeamPlayer.Player.ImageUrl : a.AccountTeamPlayer.Player.Team.ShirtStorageUrl + a.AccountTeamPlayer.Player.Team.ShirtImageUrl,
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
                           AccountTeamPlayerGameWeak = parameters.IsCurrent ?
                                       a.AccountTeamPlayerGameWeaks.Where(b => b.GameWeak.IsCurrent)
                                       .Select(b => new AccountTeamPlayerGameWeakModel
                                       {
                                           Id = b.Id,
                                           Fk_AccountTeamPlayer = b.Fk_AccountTeamPlayer,
                                           Fk_GameWeak = b.Fk_GameWeak,
                                           Fk_TeamPlayerType = b.Fk_TeamPlayerType,
                                           TrippleCaptain = b.TrippleCaptain,
                                           IsPrimary = b.IsPrimary,
                                           IsTransfer = b.IsTransfer,
                                           Order = b.Order,
                                           Points = b.Points,
                                           TeamPlayerType = new TeamPlayerTypeModel
                                           {
                                               Name = otherLang ? b.TeamPlayerType.TeamPlayerTypeLang.Name : b.TeamPlayerType.Name,
                                           }
                                       })
                                       .FirstOrDefault() :
                                       parameters.IsNextGameWeak ?
                                       a.AccountTeamPlayerGameWeaks.Where(b => b.Fk_GameWeak == parameters.Fk_NextGameWeak)
                                       .Select(b => new AccountTeamPlayerGameWeakModel
                                       {
                                           Id = b.Id,
                                           Fk_AccountTeamPlayer = b.Fk_AccountTeamPlayer,
                                           Fk_GameWeak = b.Fk_GameWeak,
                                           Fk_TeamPlayerType = b.Fk_TeamPlayerType,
                                           TrippleCaptain = b.TrippleCaptain,
                                           IsPrimary = b.IsPrimary,
                                           IsTransfer = b.IsTransfer,
                                           Order = b.Order,
                                           Points = b.Points,
                                           TeamPlayerType = new TeamPlayerTypeModel
                                           {
                                               Name = otherLang ? b.TeamPlayerType.TeamPlayerTypeLang.Name : b.TeamPlayerType.Name,
                                           }
                                       })
                                       .FirstOrDefault() :
                                       a.AccountTeamPlayerGameWeaks.Where(b => parameters.Fk_GameWeak != 0 && b.Fk_GameWeak == parameters.Fk_GameWeak)
                                       .Select(b => new AccountTeamPlayerGameWeakModel
                                       {
                                           Id = b.Id,
                                           Fk_AccountTeamPlayer = b.Fk_AccountTeamPlayer,
                                           Fk_GameWeak = b.Fk_GameWeak,
                                           Fk_TeamPlayerType = b.Fk_TeamPlayerType,
                                           TrippleCaptain = b.TrippleCaptain,
                                           IsPrimary = b.IsPrimary,
                                           IsTransfer = b.IsTransfer,
                                           Order = b.Order,
                                           Points = b.Points,
                                           TeamPlayerType = new TeamPlayerTypeModel
                                           {
                                               Name = otherLang ? b.TeamPlayerType.TeamPlayerTypeLang.Name : b.TeamPlayerType.Name,
                                           }
                                       })
                                       .FirstOrDefault(),
                           Player = new PlayerModel
                           {
                               Id = a.Fk_Player,
                               Name = otherLang ? a.Player.PlayerLang.Name : a.Player.Name,
                               ImageUrl = !string.IsNullOrEmpty(a.Player.ImageUrl) ? a.Player.StorageUrl + a.Player.ImageUrl : a.Player.Team.ShirtStorageUrl + a.Player.Team.ShirtImageUrl,
                               _365_PlayerId = a.Player._365_PlayerId,
                               Fk_Team = a.Player.Fk_Team,
                               Fk_PlayerPosition = a.Player.Fk_PlayerPosition,
                               PlayerPosition = new PlayerPositionModel
                               {
                                   Name = otherLang ? a.Player.PlayerPosition.PlayerPositionLang.Name : a.Player.PlayerPosition.Name,
                                   ImageUrl = a.Player.PlayerPosition.StorageUrl + a.Player.PlayerPosition.ImageUrl,
                               },
                               Team = new TeamModel
                               {
                                   Name = otherLang ? a.Player.Team.TeamLang.Name : a.Player.Team.Name,
                                   ImageUrl = a.Player.Team.StorageUrl + a.Player.Team.ImageUrl,
                                   ShirtImageUrl = a.Player.Team.StorageUrl + a.Player.Team.ShirtImageUrl,
                               },
                               Age = a.Player.Age,
                               PlayerNumber = a.Player.PlayerNumber,
                               ShortName = a.Player.ShortName,
                               BuyPrice = a.Player.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.BuyPrice).FirstOrDefault(),
                               SellPrice = a.Player.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.SellPrice).FirstOrDefault(),
                               SeasonScoreStates = parameters.IncludeScore && parameters.Fk_SeasonForScore > 0 ?
                                               a.Player.PlayerSeasonScoreStates
                                                .Where(b => b.Fk_Season == parameters.Fk_SeasonForScore &&
                                                            (parameters.Fk_ScoreStatesForSeason == null ||
                                                             !parameters.Fk_ScoreStatesForSeason.Any() ||
                                                             parameters.Fk_ScoreStatesForSeason.Contains(b.Fk_ScoreState)))
                                                .Select(b => new PlayerSeasonScoreStateModel
                                                {
                                                    Id = b.Id,
                                                    Fk_Player = b.Fk_Player,
                                                    Points = b.Points,
                                                    Percent = b.Percent,
                                                    Value = b.Value,
                                                    Fk_ScoreState = b.Fk_ScoreState,
                                                    Fk_Season = b.Fk_Season,
                                                    LastModifiedAt = b.LastModifiedAt,
                                                    ScoreState = new ScoreStateModel
                                                    {
                                                        Id = b.Fk_ScoreState,
                                                        Name = otherLang ? b.ScoreState.ScoreStateLang.Name : b.ScoreState.Name,
                                                    }
                                                })
                                                .ToList() : null,
                               GameWeakScoreStates = parameters.IncludeScore && parameters.Fk_GameWeakForScore > 0 ?
                                               a.Player.PlayerGameWeakScoreStates
                                                .Where(b => b.Fk_GameWeak == parameters.Fk_GameWeakForScore &&
                                                            (parameters.Fk_ScoreStatesForGameWeak == null ||
                                                             !parameters.Fk_ScoreStatesForGameWeak.Any() ||
                                                             parameters.Fk_ScoreStatesForGameWeak.Contains(b.Fk_ScoreState)))
                                                .Select(b => new PlayerGameWeakScoreStateModel
                                                {
                                                    Id = b.Id,
                                                    Fk_Player = b.Fk_Player,
                                                    Points = b.Points,
                                                    Percent = b.Percent,
                                                    Value = b.Value,
                                                    Fk_ScoreState = b.Fk_ScoreState,
                                                    Fk_GameWeak = b.Fk_GameWeak,
                                                    LastModifiedAt = b.LastModifiedAt,
                                                    GameWeak = new GameWeakModel
                                                    {
                                                        Id = b.Fk_GameWeak,
                                                        Name = otherLang ? b.GameWeak.GameWeakLang.Name : b.GameWeak.Name,
                                                    },
                                                    ScoreState = new ScoreStateModel
                                                    {
                                                        Id = b.Fk_ScoreState,
                                                        Name = otherLang ? b.ScoreState.ScoreStateLang.Name : b.ScoreState.Name,
                                                    }
                                                })
                                                .ToList() : null,
                               NextMatches = parameters.Fk_NextGameWeak == 0 ? null :
                               (a.Player
                                .Team
                                .AwayGameWeaks
                                .Any(b => b.Fk_GameWeak == parameters.Fk_NextGameWeak) ?
                               a.Player
                                .Team
                                .AwayGameWeaks
                                .Where(b => b.Fk_GameWeak == parameters.Fk_NextGameWeak)
                                .Select(b => new TeamModel
                                {
                                    Name = otherLang ? b.Home.TeamLang.Name : b.Home.Name,
                                    IsAwayTeam = false
                                })
                                .ToList() :
                                a.Player
                                .Team
                                .HomeGameWeaks
                                .Where(b => b.Fk_GameWeak == parameters.Fk_NextGameWeak)
                                .Select(b => new TeamModel
                                {
                                    Name = otherLang ? b.Away.TeamLang.Name : b.Away.Name,
                                    IsAwayTeam = true
                                })
                                .ToList())
                           },
                           AccountTeam = new AccountTeamModel
                           {
                               Name = a.AccountTeam.Name
                           },

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
