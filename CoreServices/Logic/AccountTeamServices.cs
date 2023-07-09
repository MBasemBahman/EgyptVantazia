using BaseDB;
using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.SeasonModels;
using static Contracts.EnumData.DBModelsEnum;
using static Entities.EnumData.LogicEnumData;

namespace CoreServices.Logic
{
    public class AccountTeamServices
    {
        private readonly RepositoryManager _repository;
        private readonly BaseContext _dBContext;

        public AccountTeamServices(RepositoryManager repository, BaseContext dBContext)
        {
            _repository = repository;
            _dBContext = dBContext;
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
                           Fk_FavouriteTeam = a.Fk_FavouriteTeam,
                           FavouriteTeam = a.Fk_FavouriteTeam > 0 ? new TeamModel
                           {
                               Id = a.FavouriteTeam.Id,
                               Name = otherLang ? a.FavouriteTeam.TeamLang.Name : a.FavouriteTeam.Name,
                               ImageUrl = a.FavouriteTeam.StorageUrl + a.FavouriteTeam.ImageUrl,
                               ShirtImageUrl = a.FavouriteTeam.ShirtStorageUrl + a.FavouriteTeam.ShirtImageUrl
                           } : null,
                           Name = a.Name,
                           TotalMoney = a.TotalMoney,
                           TotalTeamPrice = a.AccountTeamPlayers
                                             .Where(b => b.AccountTeamPlayerGameWeaks
                                                          .Any(c => c.GameWeak.IsNext &&
                                                                    c.AccountTeamPlayer.Fk_AccountTeam == a.Id &&
                                                                    c.IsTransfer == false))
                                             .SelectMany(b => b.Player
                                                               .PlayerPrices
                                                               .OrderByDescending(c => c.Id))
                                             .GroupBy(b => b.Fk_Player)
                                             .Select(b => b.FirstOrDefault().SellPrice)
                                             .Sum(),
                           IsVip = a.IsVip,
                           TotalPoints = ((parameters.GetMonthPlayer &&
                                     parameters.From_365_GameWeakIdValue > 0 &&
                                     parameters.To_365_GameWeakIdValue > 0) ?
                                     a.AccountTeamGameWeaks
                                      .Where(b => b.GameWeak.Season._365_CompetitionsId == parameters._365_CompetitionsId.ToString() &&
                                                  b.GameWeak._365_GameWeakIdValue >= parameters.From_365_GameWeakIdValue &&
                                                  b.GameWeak._365_GameWeakIdValue <= parameters.To_365_GameWeakIdValue)
                                      .Select(b => b.TotalPoints ?? 0)
                                      .Sum() : a.TotalPoints),
                           ImageUrl = a.StorageUrl + a.ImageUrl,
                           GlobalRanking = a.GlobalRanking,
                           GlobalRankingUpdatedAt = a.GlobalRankingUpdatedAt,
                           CountryRanking = a.CountryRanking,
                           CountryRankingUpdatedAt = a.CountryRankingUpdatedAt,
                           FavouriteTeamRanking = a.FavouriteTeamRanking,
                           FavouriteTeamRankingUpdatedAt = a.FavouriteTeamRankingUpdatedAt,
                           AccounTeamGameWeakCount = a.AccountTeamGameWeaks.Count,
                           GoldSubscriptionRanking = a.GoldSubscriptionRanking,
                           GoldSubscriptionUpdatedAt = a.GoldSubscriptionUpdatedAt,
                           UnSubscriptionRanking = a.UnSubscriptionRanking,
                           UnSubscriptionUpdatedAt = a.UnSubscriptionUpdatedAt,
                           Season = new SeasonModel
                           {
                               Name = otherLang ? a.Season.SeasonLang.Name : a.Season.Name,
                               ImageUrl = a.Season.StorageUrl + a.Season.ImageUrl,
                               _365_SeasonId = a.Season._365_SeasonId,
                               _365_CompetitionsId = a.Season._365_CompetitionsId,
                           },
                           Account = new AccountModel
                           {
                               ImageUrl = a.Account.StorageUrl + a.Account.ImageUrl,
                               FullName = a.Account.FullName,
                               Fk_Country = a.Account.Fk_Country,
                               Fk_Nationality = a.Account.Fk_Nationality,
                           },
                           PlayersCount = a.AccountTeamPlayers.Count,
                           FreeTransfer = a.FreeTransfer,
                           BenchBoost = a.BenchBoost,
                           DoubleGameWeak = a.DoubleGameWeak,
                           FreeHit = a.FreeHit,
                           Top_11 = a.Top_11,
                           TripleCaptain = a.TripleCaptain,
                           WildCard = a.WildCard,
                           Fk_AcountTeamGameWeek = a.AccountTeamGameWeaks
                                                    .Where(b => parameters.Fk_GameWeak > 0 ? b.Fk_GameWeak == parameters.Fk_GameWeak : b.GameWeak.IsCurrent == true)
                                                    .Select(a => a.Id)
                                                    .FirstOrDefault(),
                           CurrentGameWeakPoints = a.AccountTeamGameWeaks
                                                    .Where(b => parameters.Fk_GameWeak > 0 ? b.Fk_GameWeak == parameters.Fk_GameWeak : b.GameWeak.IsCurrent == true)
                                                    .Select(b => parameters.IncludeTotalPoints == false ?
                                                                 b.TotalPoints ?? 0 : b.AccountTeam
                                                                                   .AccountTeamPlayers
                                                                                   .SelectMany(c => c.AccountTeamPlayerGameWeaks)
                                                                                   .Where(c => c.Fk_GameWeak == b.Fk_GameWeak &&
                                                                                               c.IsTransfer == false &&
                                                                                               c.IsPrimary == true)
                                                                                   .SelectMany(c => c.AccountTeamPlayer
                                                                                                     .Player
                                                                                                     .PlayerGameWeakScoreStates
                                                                                                     .Where(d => d.Fk_GameWeak == b.Fk_GameWeak &&
                                                                                                                 d.Fk_ScoreState == (int)ScoreStateEnum.Total)
                                                                                                     .Select(b => (int)b.Points))
                                                                                   .Sum())
                                                    .FirstOrDefault(),
                           CurrentGameWeakGlobalRanking = a.AccountTeamGameWeaks
                                                           .Where(b => parameters.Fk_GameWeak > 0 ? b.Fk_GameWeak == parameters.Fk_GameWeak : b.GameWeak.IsCurrent == true)
                                                           .Select(b => b.GlobalRanking)
                                                           .FirstOrDefault(),
                           CurrentGameWeakTansfarePoints = a.AccountTeamGameWeaks
                                                           .Where(b => parameters.Fk_GameWeak > 0 ? b.Fk_GameWeak == parameters.Fk_GameWeak : b.GameWeak.IsCurrent == true)
                                                           .Select(b => b.TansfarePoints)
                                                           .FirstOrDefault(),
                           CurrentGameWeakCountryRanking = a.AccountTeamGameWeaks
                                                           .Where(b => parameters.Fk_GameWeak > 0 ? b.Fk_GameWeak == parameters.Fk_GameWeak : b.GameWeak.IsCurrent == true)
                                                           .Select(b => b.CountryRanking)
                                                           .FirstOrDefault(),
                           CurrentGameWeakFavouriteTeamRanking = a.AccountTeamGameWeaks
                                                           .Where(b => parameters.Fk_GameWeak > 0 ? b.Fk_GameWeak == parameters.Fk_GameWeak : b.GameWeak.IsCurrent == true)
                                                           .Select(b => b.FavouriteTeamRanking)
                                                           .FirstOrDefault(),
                           PrevGameWeakPoints = a.AccountTeamGameWeaks
                                                 .Where(a => a.GameWeak.IsPrev == true)
                                                 .Select(a => a.TotalPoints ?? 0)
                                                 .FirstOrDefault(),
                           HaveGoldSubscription = a.Account.AccountSubscriptions.Any(b => b.Fk_Subscription == (int)SubscriptionEnum.Gold &&
                                                                                              b.IsActive &&
                                                                                              b.Fk_Season == parameters.Fk_Season)
                       })
                       .Where(a => parameters.FromCurrentGameWeakPoints == null || a.CurrentGameWeakPoints >= parameters.FromCurrentGameWeakPoints)
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public int GetCurrentAcountTeamGameWeek(AccountTeamParameters parameters)
        {
            return _repository.AccountTeam
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => a.AccountTeamGameWeaks
                                                    .Where(b => parameters.Fk_GameWeak > 0 ? b.Fk_GameWeak == parameters.Fk_GameWeak : b.GameWeak.IsCurrent == true)
                                                    .Select(a => a.Id)
                                                    .FirstOrDefault()
                       )
                       .FirstOrDefault();
        }

        public IQueryable<AccountTeamModelForCalc> GetAccountTeamsForCalc(AccountTeamParameters parameters)
        {
            return _repository.AccountTeam
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new AccountTeamModelForCalc
                       {
                           Id = a.Id,
                           FreeTransfer = a.FreeTransfer,
                           BenchBoost = a.BenchBoost,
                           DoubleGameWeak = a.DoubleGameWeak,
                           FreeHit = a.FreeHit,
                           Top_11 = a.Top_11,
                           TripleCaptain = a.TripleCaptain,
                           WildCard = a.WildCard,
                           TotalMoney = a.TotalMoney,
                       });
        }

        public IQueryable<AccountTeam> GetAccountTeams(AccountTeamParameters parameters)
        {
            return _repository.AccountTeam
                       .FindAll(parameters, trackChanges: false);
        }

        public async Task<PagedList<AccountTeamModel>> GetAccountTeamPaged(
                  AccountTeamParameters parameters,
                  bool otherLang)
        {
            return await PagedList<AccountTeamModel>.ToPagedList(GetAccountTeams(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public void UpdateAccountTeamRank(int id, int fk_Season)
        {
            _repository.AccountTeam.UpdateRank(id, fk_Season);
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
            return GetAccountTeams(new AccountTeamParameters { Id = id, IncludeTotalPoints = true }, otherLang).FirstOrDefault();
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

        public void UpdateAccountTeamUpdateCards(AccountTeamsUpdateCards updateCards)
        {
            _repository.AccountTeam.UpdateAccountTeamUpdateCards(updateCards);
        }

        public void TransfareFavouriteTeam()
        {
            _repository.AccountTeam.TransfareFavouriteTeam();
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
                           TotalPoints = a.TotalPoints ?? a.AccountTeam
                                                           .AccountTeamPlayers
                                                           .SelectMany(c => c.AccountTeamPlayerGameWeaks)
                                                           .Where(c => c.Fk_GameWeak == a.Fk_GameWeak &&
                                                                       c.IsTransfer == false &&
                                                                       c.IsPrimary == true)
                                                           .SelectMany(c => c.AccountTeamPlayer
                                                                             .Player
                                                                             .PlayerGameWeakScoreStates
                                                                             .Where(d => d.Fk_GameWeak == a.Fk_GameWeak &&
                                                                                         d.Fk_ScoreState == (int)ScoreStateEnum.Total)
                                                                             .Select(b => (int)b.Points))
                                                           .Sum(),
                           PrevPoints = a.PrevPoints,
                           SeasonTotalPoints = a.SeasonTotalPoints,
                           WildCard = a.WildCard,
                           Fk_AccountTeam = a.Fk_AccountTeam,
                           Fk_GameWeak = a.Fk_GameWeak,
                           BenchBoost = a.BenchBoost,
                           DoubleGameWeak = a.DoubleGameWeak,
                           TansfarePoints = a.TansfarePoints,
                           BenchPoints = a.BenchPoints,
                           TansfareCount = a.AccountTeam.PlayerTransfers.Count(b => b.Fk_GameWeak == a.Fk_GameWeak && b.TransferTypeEnum == TransferTypeEnum.Buying),
                           Top_11 = a.Top_11,
                           GlobalRanking = a.GlobalRanking,
                           GlobalRankingUpdatedAt = a.GlobalRankingUpdatedAt,
                           SeasonGlobalRanking = a.SeasonGlobalRanking,
                           SeasonGoldSubscriptionRanking = a.SeasonGoldSubscriptionRanking,
                           SeasonUnSubscriptionRanking = a.SeasonUnSubscriptionRanking,
                           CountryRanking = a.CountryRanking,
                           CountryRankingUpdatedAt = a.CountryRankingUpdatedAt,
                           FavouriteTeamRanking = a.FavouriteTeamRanking,
                           FavouriteTeamRankingUpdatedAt = a.FavouriteTeamRankingUpdatedAt,
                           AvailableBenchBoost = !a.BenchBoost,
                           AvailableDoubleGameWeak = !a.DoubleGameWeak,
                           AvailableFreeHit = !a.FreeHit,
                           AvailableTop_11 = !a.Top_11,
                           AvailableWildCard = !a.WildCard,
                           TripleCaptain = a.TripleCaptain,
                           GameWeak = new GameWeakModel
                           {
                               Id = a.GameWeak.Id,
                               Name = otherLang ? a.GameWeak.GameWeakLang.Name : a.GameWeak.Name,
                               _365_GameWeakId = a.GameWeak._365_GameWeakId,
                               Fk_Season = a.GameWeak.Fk_Season,
                               Season = new SeasonModel
                               {
                                   Name = otherLang ? a.GameWeak.Season.SeasonLang.Name : a.GameWeak.Season.Name
                               }
                           },
                           NextGameWeak = a.GameWeak.IsCurrent == false && parameters.IncludeNextAndPrevGameWeek && a.AccountTeam.AccountTeamGameWeaks.Any(b => b.GameWeak._365_GameWeakId == (a.GameWeak._365_GameWeakIdValue + 1).ToString()) ?
                           a.AccountTeam
                            .AccountTeamGameWeaks
                            .Where(b => b.GameWeak._365_GameWeakId == (a.GameWeak._365_GameWeakIdValue + 1).ToString())
                            .Select(b => new GameWeakModel
                            {
                                Id = b.GameWeak.Id,
                                Name = otherLang ? b.GameWeak.GameWeakLang.Name : b.GameWeak.Name,
                                _365_GameWeakId = b.GameWeak._365_GameWeakId,
                                Fk_Season = b.GameWeak.Fk_Season
                            }).FirstOrDefault() : null,
                           PrevGameWeak = parameters.IncludeNextAndPrevGameWeek && a.AccountTeam.AccountTeamGameWeaks.Any(b => b.GameWeak._365_GameWeakId == (a.GameWeak._365_GameWeakIdValue - 1).ToString()) ?
                           a.AccountTeam
                            .AccountTeamGameWeaks
                            .Where(b => b.GameWeak._365_GameWeakId == (a.GameWeak._365_GameWeakIdValue - 1).ToString())
                            .Select(b => new GameWeakModel
                            {
                                Id = b.GameWeak.Id,
                                Name = otherLang ? b.GameWeak.GameWeakLang.Name : b.GameWeak.Name,
                                _365_GameWeakId = b.GameWeak._365_GameWeakId,
                                Fk_Season = b.GameWeak.Fk_Season
                            }).FirstOrDefault() : null,
                           AccountTeam = new AccountTeamModel
                           {
                               Id = a.Fk_AccountTeam,
                               Name = a.AccountTeam.Name,
                               IsVip = a.AccountTeam.IsVip,
                               Account = new AccountModel
                               {
                                   Fk_Country = a.AccountTeam.Account.Fk_Country,
                                   Name = a.AccountTeam.Account.FullName,
                               },
                               Fk_Account = a.AccountTeam.Fk_Account,
                               Fk_FavouriteTeam = a.AccountTeam.Fk_FavouriteTeam,
                               TotalMoney = a.AccountTeam.TotalMoney,
                               TotalPoints = a.AccountTeam.TotalPoints,
                               GlobalRanking = a.AccountTeam.GlobalRanking,
                               CountryRanking = a.AccountTeam.CountryRanking,
                               FavouriteTeamRanking = a.AccountTeam.FavouriteTeamRanking,
                               UnSubscriptionRanking = a.AccountTeam.UnSubscriptionRanking,
                               GoldSubscriptionRanking = a.AccountTeam.GoldSubscriptionRanking,
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public IQueryable<AccountTeamGameWeakModelForCalc> GetAccountTeamGameWeaksForCalc(AccountTeamGameWeakParameters parameters)
        {
            return _repository.AccountTeamGameWeak
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new AccountTeamGameWeakModelForCalc
                       {
                           Id = a.Id,
                           FreeHit = a.FreeHit,
                           WildCard = a.WildCard,
                           BenchBoost = a.BenchBoost,
                           DoubleGameWeak = a.DoubleGameWeak,
                           Top_11 = a.Top_11,
                           TripleCaptain = a.TripleCaptain,
                       });
        }

        public IQueryable<AccountTeamGameWeak> GetAccountTeamGameWeaks(AccountTeamGameWeakParameters parameters)
        {
            return _repository.AccountTeamGameWeak
                       .FindAll(parameters, trackChanges: false);
        }

        public async Task<PagedList<AccountTeamGameWeakModel>> GetAccountTeamGameWeakPaged(
                  AccountTeamGameWeakParameters parameters,
                  bool otherLang)
        {
            return await PagedList<AccountTeamGameWeakModel>.ToPagedList(GetAccountTeamGameWeaks(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public void UpdateAccountTeamGameWeakRank(int fk_AccountTeam, int fk_GameWeek)
        {
            _repository.AccountTeamGameWeak.UpdateRank(fk_AccountTeam, fk_GameWeek);
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
            return GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters { Id = id }, otherLang).FirstOrDefault();
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

        public double GetAverageGameWeakPoints(int fk_GameWeak)
        {
            return _repository.AccountTeamGameWeak.GetAverageGameWeakPoints(fk_GameWeak);
        }

        public List<AccountTeamModel> GetPrivateLeaguesPoints(int fk_Season, int fk_PrivateLeague, int _365_GameWeakIdValue)
        {
            return _repository.AccountTeam.GetPrivateLeaguesPoints(fk_Season, fk_PrivateLeague, _365_GameWeakIdValue);
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
                           IsPrimary = a.IsPrimary,
                           IsTransfer = a.IsTransfer,
                           Order = a.Order,
                           PlayerName = a.AccountTeamPlayer.Player.Name,
                           Points = a.Points ?? a.AccountTeamPlayer
                                                 .Player
                                                 .PlayerGameWeakScoreStates
                                                 .Where(b => b.Fk_GameWeak == a.Fk_GameWeak &&
                                                             b.Fk_ScoreState == (int)ScoreStateEnum.Total)
                                                 .Select(b => (int)b.Points)
                                                 .FirstOrDefault(),
                           HavePoints = a.HavePoints,
                           HavePointsInTotal = a.Points == null ? a.IsPrimary : a.HavePointsInTotal,
                           IsPlayed = a.AccountTeamPlayer
                                       .Player
                                       .Team
                                       .HomeGameWeaks.Any(b => b.Fk_GameWeak == a.Fk_GameWeak &&
                                                               b.StartTime <= DateTime.UtcNow.AddHours(2)) ||
                                       a.AccountTeamPlayer
                                       .Player
                                       .Team
                                       .AwayGameWeaks.Any(b => b.Fk_GameWeak == a.Fk_GameWeak &&
                                                               b.StartTime <= DateTime.UtcNow.AddHours(2)),
                           IsDelayed = a.AccountTeamPlayer
                                       .Player
                                       .Team
                                       .HomeGameWeaks.Any(b => b.Fk_GameWeak == a.Fk_GameWeak &&
                                                               b.IsDelayed) ||
                                       a.AccountTeamPlayer
                                       .Player
                                       .Team
                                       .AwayGameWeaks.Any(b => b.Fk_GameWeak == a.Fk_GameWeak &&
                                                               b.IsDelayed),
                           NotHaveMatch = !a.AccountTeamPlayer
                                           .Player
                                           .Team
                                           .HomeGameWeaks.Any(b => b.Fk_GameWeak == a.Fk_GameWeak) &&
                                          !a.AccountTeamPlayer
                                           .Player
                                           .Team
                                           .AwayGameWeaks.Any(b => b.Fk_GameWeak == a.Fk_GameWeak),
                           IsParticipate = _dBContext.PlayerGameWeaks.Any(b => b.Fk_Player == a.AccountTeamPlayer.Fk_Player &&
                                                                               b.TeamGameWeak.Fk_GameWeak == a.Fk_GameWeak &&
                                                                               b.TeamGameWeak.StartTime <= DateTime.UtcNow.AddHours(2) &&
                                                                               (b.TeamGameWeak.IsEnded == false ||
                                                                                b.TotalPoints != 0 ||
                                                                                b.Ranking != 0 ||
                                                                                b.PlayerGameWeakScores.Any(c => c.Fk_ScoreType == (int)ScoreTypeEnum.Minutes))),
                           Top15 = a.AccountTeamPlayer
                                    .Player
                                    .PlayerGameWeakScoreStates
                                    .Where(b => b.Fk_GameWeak == a.Fk_GameWeak && b.Top15 != null)
                                    .Select(b => b.Top15)
                                    .FirstOrDefault(),
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
                               Fk_Player = a.AccountTeamPlayer.Fk_Player,
                               Player = new PlayerModel
                               {
                                   Id = a.AccountTeamPlayer.Fk_Player,
                                   InExternalTeam = a.AccountTeamPlayer.Player.InExternalTeam,
                                   Fk_PlayerPosition = a.AccountTeamPlayer.Player.Fk_PlayerPosition,
                                   Name = otherLang ? a.AccountTeamPlayer.Player.PlayerLang.Name : a.AccountTeamPlayer.Player.Name,
                                   ShortName = otherLang ? a.AccountTeamPlayer.Player.PlayerLang.ShortName : a.AccountTeamPlayer.Player.ShortName,
                                   ImageUrl = !string.IsNullOrEmpty(a.AccountTeamPlayer.Player.ImageUrl) ? a.AccountTeamPlayer.Player.StorageUrl + a.AccountTeamPlayer.Player.ImageUrl : a.AccountTeamPlayer.Player.Team.ShirtStorageUrl + a.AccountTeamPlayer.Player.Team.ShirtImageUrl,
                                   _365_PlayerId = a.AccountTeamPlayer.Player._365_PlayerId,
                                   PlayerMarks = a.AccountTeamPlayer
                                                  .Player
                                                  .PlayerMarks
                                                  .Where(b => b.Count > b.Used)
                                                  .Select(b => new PlayerMarkModel
                                                  {
                                                      Count = b.Count,
                                                      Used = b.Used,
                                                      Mark = new MarkModel
                                                      {
                                                          ImageUrl = b.Mark.StorageUrl + b.Mark.ImageUrl,
                                                          Id = b.Mark.Id,
                                                          Name = otherLang ? b.Mark.MarkLang.Name : b.Mark.Name
                                                      }
                                                  })
                                                  .ToList()
                               },
                               AccountTeam = new AccountTeamModel
                               {
                                   Id = a.AccountTeamPlayer.Fk_AccountTeam,
                                   Name = a.AccountTeamPlayer.AccountTeam.Name,
                                   IsVip = a.AccountTeamPlayer.AccountTeam.IsVip,
                               }
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public IQueryable<AccountTeamPlayerGameWeak> GetAccountTeamPlayerGameWeaks(AccountTeamPlayerGameWeakParameters parameters)
        {
            return _repository.AccountTeamPlayerGameWeak
                       .FindAll(parameters, trackChanges: false);
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

        public void ResetTeamPlayers(int fk_AccountTeam, int fk_GameWeak, int fk_AccountTeamGameWeak)
        {
            _repository.AccountTeamPlayerGameWeak.ResetTeamPlayers(fk_AccountTeam, fk_GameWeak, fk_AccountTeamGameWeak);
        }

        public void ResetAccountTeamPlayerGameWeakPoints(int fk_AccountTeam, int fk_GameWeak)
        {
            _repository.AccountTeamPlayerGameWeak.ResetPoints(fk_AccountTeam, fk_GameWeak);
        }

        public async Task DeleteAccountTeamPlayerGameWeak(int id)
        {
            AccountTeamPlayerGameWeak AccountTeamPlayerGameWeak = await FindAccountTeamPlayerGameWeakbyId(id, trackChanges: true);
            _repository.AccountTeamPlayerGameWeak.Delete(AccountTeamPlayerGameWeak);
        }

        public AccountTeamPlayerGameWeakModel GetAccountTeamPlayerGameWeakbyId(int id, bool otherLang)
        {
            return GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetAccountTeamPlayerGameWeakCount()
        {
            return _repository.AccountTeamPlayerGameWeak.Count();
        }

        public void ResetAccountTeamPlayer(int fk_AccountTeam, int fk_GameWeak)
        {
            _repository.AccountTeamPlayerGameWeak.ResetAccountTeamPlayer(fk_AccountTeam, fk_GameWeak);
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
                           AccountTeamPlayerGameWeak = parameters.IsCurrent == true ?
                                       a.AccountTeamPlayerGameWeaks.Where(b => b.GameWeak.IsCurrent)
                                       .Select(b => new AccountTeamPlayerGameWeakModel
                                       {
                                           Id = b.Id,
                                           Fk_AccountTeamPlayer = b.Fk_AccountTeamPlayer,
                                           Fk_GameWeak = b.Fk_GameWeak,
                                           Fk_TeamPlayerType = b.Fk_TeamPlayerType,
                                           IsPrimary = b.IsPrimary,
                                           IsTransfer = b.IsTransfer,
                                           Order = b.Order,
                                           Points = b.Points ?? b.AccountTeamPlayer
                                                                 .Player
                                                                 .PlayerGameWeakScoreStates
                                                                 .Where(c => c.Fk_GameWeak == b.Fk_GameWeak &&
                                                                             c.Fk_ScoreState == (int)ScoreStateEnum.Total)
                                                                 .Select(c => (int)c.Points)
                                                                 .FirstOrDefault(),
                                           TeamPlayerType = new TeamPlayerTypeModel
                                           {
                                               Name = otherLang ? b.TeamPlayerType.TeamPlayerTypeLang.Name : b.TeamPlayerType.Name,
                                           },
                                           HavePoints = b.HavePoints,
                                           HavePointsInTotal = b.Points == null ? b.IsPrimary : b.HavePointsInTotal,
                                           IsPlayed = b.AccountTeamPlayer
                                                       .Player
                                                       .Team
                                                       .HomeGameWeaks
                                                       .Any(c => c.Fk_GameWeak == b.Fk_GameWeak &&
                                                                 c.StartTime <= DateTime.UtcNow.AddHours(2)) ||
                                                       b.AccountTeamPlayer
                                                       .Player
                                                       .Team
                                                       .AwayGameWeaks
                                                       .Any(c => c.Fk_GameWeak == b.Fk_GameWeak &&
                                                                 c.StartTime <= DateTime.UtcNow.AddHours(2)),
                                           Top15 = a.Player
                                                    .PlayerGameWeakScoreStates
                                                    .Where(c => c.Fk_GameWeak == b.Fk_GameWeak && c.Top15 != null)
                                                    .Select(c => c.Top15)
                                                    .FirstOrDefault()
                                       })
                                       .FirstOrDefault() :
                                       parameters.IsNextGameWeak == true ?
                                       a.AccountTeamPlayerGameWeaks.Where(b => b.GameWeak.IsNext)
                                       .Select(b => new AccountTeamPlayerGameWeakModel
                                       {
                                           Id = b.Id,
                                           Fk_AccountTeamPlayer = b.Fk_AccountTeamPlayer,
                                           Fk_GameWeak = b.Fk_GameWeak,
                                           Fk_TeamPlayerType = b.Fk_TeamPlayerType,
                                           IsPrimary = b.IsPrimary,
                                           IsTransfer = b.IsTransfer,
                                           Order = b.Order,
                                           Points = b.Points ?? b.AccountTeamPlayer
                                                                 .Player
                                                                 .PlayerGameWeakScoreStates
                                                                 .Where(c => c.Fk_GameWeak == b.Fk_GameWeak &&
                                                                             c.Fk_ScoreState == (int)ScoreStateEnum.Total)
                                                                 .Select(c => (int)c.Points)
                                                                 .FirstOrDefault(),
                                           TeamPlayerType = new TeamPlayerTypeModel
                                           {
                                               Name = otherLang ? b.TeamPlayerType.TeamPlayerTypeLang.Name : b.TeamPlayerType.Name,
                                           },
                                           HavePoints = b.HavePoints,
                                           HavePointsInTotal = b.Points == null ? b.IsPrimary : b.HavePointsInTotal,
                                           IsPlayed = b.AccountTeamPlayer
                                                       .Player
                                                       .Team
                                                       .HomeGameWeaks
                                                       .Any(c => c.Fk_GameWeak == b.Fk_GameWeak &&
                                                                 c.StartTime <= DateTime.UtcNow.AddHours(2)) ||
                                                       b.AccountTeamPlayer
                                                       .Player
                                                       .Team
                                                       .AwayGameWeaks
                                                       .Any(c => c.Fk_GameWeak == b.Fk_GameWeak &&
                                                                 c.StartTime <= DateTime.UtcNow.AddHours(2)),
                                           Top15 = a.Player
                                                    .PlayerGameWeakScoreStates
                                                    .Where(c => c.Fk_GameWeak == b.Fk_GameWeak && c.Top15 != null)
                                                    .Select(c => c.Top15)
                                                    .FirstOrDefault()
                                       })
                                       .FirstOrDefault() :
                                       a.AccountTeamPlayerGameWeaks.Where(b => parameters.Fk_GameWeak != 0 && b.Fk_GameWeak == parameters.Fk_GameWeak)
                                       .Select(b => new AccountTeamPlayerGameWeakModel
                                       {
                                           Id = b.Id,
                                           Fk_AccountTeamPlayer = b.Fk_AccountTeamPlayer,
                                           Fk_GameWeak = b.Fk_GameWeak,
                                           Fk_TeamPlayerType = b.Fk_TeamPlayerType,
                                           IsPrimary = b.IsPrimary,
                                           IsTransfer = b.IsTransfer,
                                           Order = b.Order,
                                           Points = b.Points ?? b.AccountTeamPlayer
                                                                 .Player
                                                                 .PlayerGameWeakScoreStates
                                                                 .Where(c => c.Fk_GameWeak == b.Fk_GameWeak &&
                                                                             c.Fk_ScoreState == (int)ScoreStateEnum.Total)
                                                                 .Select(c => (int)c.Points)
                                                                 .FirstOrDefault(),
                                           HavePoints = b.HavePoints,
                                           HavePointsInTotal = b.Points == null ? b.IsPrimary : b.HavePointsInTotal,
                                           IsPlayed = b.AccountTeamPlayer
                                                       .Player
                                                       .Team
                                                       .HomeGameWeaks
                                                       .Any(c => c.Fk_GameWeak == b.Fk_GameWeak &&
                                                                 c.StartTime <= DateTime.UtcNow.AddHours(2)) ||
                                                       b.AccountTeamPlayer
                                                       .Player
                                                       .Team
                                                       .AwayGameWeaks
                                                       .Any(c => c.Fk_GameWeak == b.Fk_GameWeak &&
                                                                 c.StartTime <= DateTime.UtcNow.AddHours(2)),
                                           TeamPlayerType = new TeamPlayerTypeModel
                                           {
                                               Name = otherLang ? b.TeamPlayerType.TeamPlayerTypeLang.Name : b.TeamPlayerType.Name,
                                           },
                                           Top15 = a.Player
                                                    .PlayerGameWeakScoreStates
                                                    .Where(c => c.Fk_GameWeak == b.Fk_GameWeak && c.Top15 != null)
                                                    .Select(c => c.Top15)
                                                    .FirstOrDefault()
                                       })
                                       .FirstOrDefault(),
                           Player = new PlayerModel
                           {
                               Id = a.Fk_Player,
                               Name = otherLang ? a.Player.PlayerLang.Name : a.Player.Name,
                               ShortName = otherLang ? a.Player.PlayerLang.ShortName : a.Player.ShortName,
                               ImageUrl = !string.IsNullOrEmpty(a.Player.ImageUrl) ? a.Player.StorageUrl + a.Player.ImageUrl : a.Player.Team.ShirtStorageUrl + a.Player.Team.ShirtImageUrl,
                               _365_PlayerId = a.Player._365_PlayerId,
                               Fk_Team = a.Player.Fk_Team,
                               Fk_PlayerPosition = a.Player.Fk_PlayerPosition,
                               InExternalTeam = a.Player.InExternalTeam,
                               Top15 = a.Player
                                        .PlayerSeasonScoreStates
                                        .Where(b => b.Season.IsCurrent && b.Top15 != null)
                                        .Select(b => b.Top15)
                                        .FirstOrDefault(),
                               PlayerMarks = a.Player
                                              .PlayerMarks
                                              .Where(b => b.Count > b.Used)
                                              .Select(b => new PlayerMarkModel
                                              {
                                                  Count = b.Count,
                                                  Used = b.Used,
                                                  Mark = new MarkModel
                                                  {
                                                      ImageUrl = b.Mark.StorageUrl + b.Mark.ImageUrl,
                                                      Id = b.Mark.Id,
                                                      Name = otherLang ? b.Mark.MarkLang.Name : b.Mark.Name
                                                  }
                                              })
                                              .ToList(),
                               PlayerPosition = new PlayerPositionModel
                               {
                                   Name = otherLang ? a.Player.PlayerPosition.PlayerPositionLang.Name : a.Player.PlayerPosition.Name,
                                   ShortName = otherLang ? a.Player.PlayerPosition.PlayerPositionLang.ShortName : a.Player.PlayerPosition.ShortName,
                                   ImageUrl = a.Player.PlayerPosition.StorageUrl + a.Player.PlayerPosition.ImageUrl,
                               },
                               Team = new TeamModel
                               {
                                   Name = otherLang ? a.Player.Team.TeamLang.Name : a.Player.Team.Name,
                                   ShortName = otherLang ? a.Player.Team.TeamLang.ShortName : a.Player.Team.ShortName,
                                   ImageUrl = a.Player.Team.StorageUrl + a.Player.Team.ImageUrl,
                                   ShirtImageUrl = a.Player.Team.StorageUrl + a.Player.Team.ShirtImageUrl,
                               },
                               Age = a.Player.Age,
                               PlayerNumber = a.Player.PlayerNumber,
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
                               NextMatch = parameters.IncludeNextMatch ?
                               _dBContext.Set<TeamGameWeak>()
                                          .Where(b => b.IsActive &&
                                                      b.StartTime >= DateTime.UtcNow.AddHours(2) &&
                                                      (b.Fk_Away == a.Player.Fk_Team || b.Fk_Home == a.Player.Fk_Team))
                                          .OrderBy(b => b.StartTime)
                                          .Select(b => b.Fk_Home != a.Player.Fk_Team ? new TeamModel
                                          {
                                              Name = otherLang ? b.Home.TeamLang.Name : b.Home.Name,
                                              ShortName = otherLang ? b.Home.TeamLang.ShortName : b.Home.ShortName,
                                              IsAwayTeam = false
                                          } : new TeamModel
                                          {
                                              Name = otherLang ? b.Away.TeamLang.Name : b.Away.Name,
                                              ShortName = otherLang ? b.Away.TeamLang.ShortName : b.Away.ShortName,
                                              IsAwayTeam = true
                                          })
                                          .FirstOrDefault() : null,
                               NextMatches = parameters.IncludeNextMatch ?
                               _dBContext.Set<TeamGameWeak>()
                                          .Where(b => b.IsActive &&
                                                      b.StartTime >= DateTime.UtcNow.AddHours(2) &&
                                                      (parameters.FromDeadLine == null || b.StartTime >= parameters.FromDeadLine) &&
                                                      (parameters.ToDeadLine == null || b.StartTime <= parameters.ToDeadLine) &&
                                                      (b.Fk_Away == a.Player.Fk_Team || b.Fk_Home == a.Player.Fk_Team))
                                          .OrderBy(b => b.StartTime)
                                          .Select(b => b.Fk_Home != a.Player.Fk_Team ? new TeamModel
                                          {
                                              Name = otherLang ? b.Home.TeamLang.Name : b.Home.Name,
                                              ShortName = otherLang ? b.Home.TeamLang.ShortName : b.Home.ShortName,
                                              IsAwayTeam = false
                                          } : new TeamModel
                                          {
                                              Name = otherLang ? b.Away.TeamLang.Name : b.Away.Name,
                                              ShortName = otherLang ? b.Away.TeamLang.ShortName : b.Away.ShortName,
                                              IsAwayTeam = true
                                          })
                                          .ToList() : null,
                           },
                           AccountTeam = new AccountTeamModel
                           {
                               Name = a.AccountTeam.Name,
                               IsVip = a.AccountTeam.IsVip
                           },

                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public IQueryable<AccountTeamPlayer> GetAccountTeamPlayers(AccountTeamPlayerParameters parameters)
        {
            return _repository.AccountTeamPlayer
                       .FindAll(parameters, trackChanges: false);
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
            return GetAccountTeamPlayers(new AccountTeamPlayerParameters { Id = id }, otherLang).FirstOrDefault();
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
            return GetTeamPlayerTypes(new RequestParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetTeamPlayerTypeCount()
        {
            return _repository.TeamPlayerType.Count();
        }
        #endregion
    }
}
