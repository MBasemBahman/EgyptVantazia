using API.Controllers;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.AccountTeamModels;
using FantasyLogic;
using IntegrationWith365;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static Entities.EnumData.LogicEnumData;

namespace API.Areas.AccountTeamArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("AccountTeam")]
    [ApiExplorerSettings(GroupName = "AccountTeam")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class AccountTeamController : ExtendControllerBase
    {
        private readonly _365Services _365Services;
        private readonly FantasyUnitOfWork _fantasyUnitOfWork;

        public AccountTeamController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        _365Services _365Services,
        IOptions<AppSettings> appSettings,
        FantasyUnitOfWork fantasyUnitOfWork) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        {
            this._365Services = _365Services;
            _fantasyUnitOfWork = fantasyUnitOfWork;
        }

        [HttpGet]
        [Route(nameof(GetAccountTeams))]
        public async Task<IEnumerable<AccountTeamModel>> GetAccountTeams(
        [FromQuery] AccountTeamParameters parameters)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            if (parameters.OrderBy.Contains("globalRanking") ||
                parameters.OrderBy.Contains("countryRanking") ||
                parameters.OrderBy.Contains("favouriteTeamRanking"))
            {
                parameters.FromGlobalRanking = 1;

                if (parameters.OrderBy.Contains("globalRanking"))
                {
                    parameters.OrderBy = "totalPoints desc";
                }

                if (parameters.OrderBy.Contains("countryRanking"))
                {
                    parameters.Fk_Country = auth.Fk_Country;
                    parameters.OrderBy = "totalPoints desc";
                }

                if (parameters.OrderBy.Contains("favouriteTeamRanking"))
                {
                    parameters.Fk_FavouriteTeam = auth.Fk_FavouriteTeam;
                    parameters.OrderBy = "totalPoints desc";
                }
            }

            if (parameters.OrderBy.Contains("currentGameWeakGlobalRanking") ||
                parameters.OrderBy.Contains("currentGameWeakCountryRanking") ||
                parameters.OrderBy.Contains("currentGameWeakFavouriteTeamRanking"))
            {
                parameters.FromCurrentGameWeakPoints = 1;

                if (parameters.OrderBy.Contains("currentGameWeakGlobalRanking"))
                {
                    parameters.OrderBy = "currentGameWeakPoints desc";
                }

                if (parameters.OrderBy.Contains("currentGameWeakCountryRanking"))
                {
                    parameters.Fk_Country = auth.Fk_Country;
                    parameters.OrderBy = "currentGameWeakPoints desc";
                }

                if (parameters.OrderBy.Contains("currentGameWeakFavouriteTeamRanking"))
                {
                    parameters.Fk_FavouriteTeam = auth.Fk_FavouriteTeam;
                    parameters.OrderBy = "currentGameWeakPoints desc";
                }
            }

            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<AccountTeamModel> data = await _unitOfWork.AccountTeam.GetAccountTeamPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpGet]
        [Route(nameof(GetAccountTeamById))]
        public AccountTeamModel GetAccountTeamById(
        [FromQuery, BindRequired] int id,
        [FromQuery] bool includeGameWeakPoints,
        [FromQuery] int fk_GameWeak)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AccountTeamModel data = _unitOfWork.AccountTeam.GetAccountTeambyId(id, otherLang);
            GameWeakModel gameWeak = null;

            if (fk_GameWeak == 0)
            {
                gameWeak = _unitOfWork.Season.GetCurrentGameWeak();

                fk_GameWeak = gameWeak.Id;
            }

            _unitOfWork.AccountTeam.UpdateAccountTeamRank(id);

            _unitOfWork.AccountTeam.UpdateAccountTeamGameWeakRank(id, fk_GameWeak);

            if (data.Fk_AcountTeamGameWeek > 0)
            {
                gameWeak ??= _unitOfWork.Season.GetGameWeakbyId(fk_GameWeak, otherLang);

                AccountTeamCustemClac clac = _fantasyUnitOfWork.AccountTeamCalc.AccountTeamPlayersCalculations(data.Fk_AcountTeamGameWeek, id, gameWeak, gameWeak.Fk_Season, false);
                if (clac != null)
                {
                    data.CurrentGameWeakPoints = clac.TotalPoints ?? 0;
                    data.PrevGameWeakPoints = clac.PrevPoints;
                }
            }

            if (data != null && includeGameWeakPoints)
            {
                data.AverageGameWeakPoints = _unitOfWork.AccountTeam.GetAverageGameWeakPoints(fk_GameWeak);

                data.BestAccountTeamGameWeak = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                {
                    Fk_GameWeak = fk_GameWeak,
                    PointsFrom = 1
                }, otherLang: otherLang)
                    .Where(a => a.TotalPoints > 0)
                    .OrderByDescending(a => a.TotalPoints)
                    .FirstOrDefault();
            }

            return data;
        }

        [HttpGet]
        [Route(nameof(GetMyAccountTeam))]
        public AccountTeamModel GetMyAccountTeam([FromQuery] bool includeGameWeakPoints)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            SeasonModel currentSeason = _unitOfWork.Season.GetCurrentSeason();
            if (currentSeason == null)
            {
                throw new Exception("Season not started yet!");
            }

            AccountTeamModel currentTeam = _unitOfWork.AccountTeam.GetCurrentTeam(auth.Fk_Account, currentSeason.Id);
            if (currentTeam != null && includeGameWeakPoints)
            {
                GameWeakModel currentGameWeak = _unitOfWork.Season.GetCurrentGameWeak();

                currentTeam.AverageGameWeakPoints = _unitOfWork.AccountTeam.GetAverageGameWeakPoints(currentGameWeak.Id);

                currentTeam.BestAccountTeamGameWeak = _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                {
                    Fk_GameWeak = currentGameWeak.Id
                }, otherLang: otherLang)
                    .OrderByDescending(a => a.TotalPoints)
                    .FirstOrDefault();
            }

            return currentTeam ?? throw new Exception("Please create your team!");
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<AccountTeamModel> Create([FromForm] AccountTeamCreateModel model)
        {
            //throw new Exception("نعمل حاليا على على بعض تحديثات الفرق واللاعبين والأسعار, الرجاء المحاوله في وقت لاحق");

            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            SeasonModel currentSeason = _unitOfWork.Season.GetCurrentSeason();
            GameWeakModel nextGameWeak = _unitOfWork.Season.GetNextGameWeak();

            AccountTeam accountTeam = _mapper.Map<AccountTeam>(model);
            accountTeam.CreatedBy = auth.Name;
            accountTeam.Fk_Account = auth.Fk_Account;
            accountTeam.Fk_Season = currentSeason.Id;
            accountTeam.AccountTeamGameWeaks = new List<AccountTeamGameWeak>
            {
                new AccountTeamGameWeak
                {
                    Fk_GameWeak = nextGameWeak.Id,
                }
            };

            if (model.ImageFile != null)
            {
                accountTeam.ImageUrl = await _unitOfWork.AccountTeam.UploadAccountTeamImage(_environment.WebRootPath, model.ImageFile);
                accountTeam.StorageUrl = GetBaseUri();
            }

            _unitOfWork.AccountTeam.CreateAccountTeam(accountTeam);
            await _unitOfWork.Save();

            return _unitOfWork.AccountTeam.GetAccountTeambyId(accountTeam.Id, otherLang);
        }

        [HttpPut]
        [Route(nameof(Edit))]
        public async Task<AccountTeamModel> Edit(
            [FromQuery, BindRequired] int id,
            [FromForm] AccountTeamCreateModel model)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            AccountTeam accountTeam = await _unitOfWork.AccountTeam.FindAccountTeambyId(id, trackChanges: true);

            _ = _mapper.Map(model, accountTeam);
            accountTeam.LastModifiedBy = auth.Name;

            if (model.ImageFile != null)
            {
                accountTeam.ImageUrl = await _unitOfWork.AccountTeam.UploadAccountTeamImage(_environment.WebRootPath, model.ImageFile);
                accountTeam.StorageUrl = GetBaseUri();
            }

            await _unitOfWork.Save();

            return _unitOfWork.AccountTeam.GetAccountTeambyId(accountTeam.Id, otherLang);
        }


        [HttpPost]
        [Route(nameof(ActivateCard))]
        public async Task<bool> ActivateCard([FromQuery, BindRequired] CardTypeEnum cardTypeEnum)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
            _ = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SeasonModel currentSeason = _unitOfWork.Season.GetCurrentSeason();
            if (currentSeason == null)
            {
                throw new Exception("Season not started yet!");
            }

            GameWeakModel nextGameWeak = _unitOfWork.Season.GetNextGameWeak();
            if (nextGameWeak == null || nextGameWeak._365_GameWeakId_Parsed == null)
            {
                throw new Exception("Game Weak not started yet!");
            }

            AccountTeamModel currentTeam = _unitOfWork.AccountTeam.GetCurrentTeam(auth.Fk_Account, currentSeason.Id);
            if (currentTeam == null)
            {
                throw new Exception("Please create your team!");
            }

            AccountTeamGameWeakModel teamGameWeak = _unitOfWork.AccountTeam.GetTeamGameWeak(auth.Fk_Account, nextGameWeak.Id);
            if (currentTeam == null)
            {
                throw new Exception("Game Weak not started yet!");
            }

            if ((cardTypeEnum == CardTypeEnum.WildCard ||
                cardTypeEnum == CardTypeEnum.FreeHit) &&
                _unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                {
                    Fk_AccountTeam = currentTeam.Id,
                }, otherLang: false).Count() == 1)
            {
                throw new Exception("You cannot activate this card, because you in first gameweek!");
            }

            if (teamGameWeak.BenchBoost ||
                teamGameWeak.DoubleGameWeak ||
                teamGameWeak.WildCard ||
                teamGameWeak.FreeHit ||
                teamGameWeak.Top_11 ||
                teamGameWeak.TripleCaptain)
            {
                throw new Exception("You already use card in this gameweek!");
            }

            int gameWeakFrom = 1;
            int gameWeakTo = 17;

            if (nextGameWeak._365_GameWeakId_Parsed >= 18)
            {
                gameWeakFrom = 18;
                gameWeakTo = 34;
            }

            if (cardTypeEnum == CardTypeEnum.BenchBoost)
            {
                if (currentTeam.BenchBoost <= 0)
                {
                    throw new Exception("You not have valid card!");
                }

                //if (_unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                //{
                //    Fk_AccountTeam = currentTeam.Id,
                //    GameWeakFrom = gameWeakFrom,
                //    GameWeakTo = gameWeakTo,
                //    BenchBoost = true
                //}, otherLang: false).Count() >= 2)
                //{
                //    throw new Exception("You already use card in this half of season!");
                //}
            }
            else if (cardTypeEnum == CardTypeEnum.FreeHit)
            {
                if (currentTeam.FreeHit <= 0)
                {
                    throw new Exception("You not have valid card!");
                }

                //if (_unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                //{
                //    Fk_AccountTeam = currentTeam.Id,
                //    GameWeakFrom = gameWeakFrom,
                //    GameWeakTo = gameWeakTo,
                //    FreeHit = true
                //}, otherLang: false).Count() >= 1)
                //{
                //    throw new Exception("You already use card in this half of season!");
                //}
            }
            else if (cardTypeEnum == CardTypeEnum.WildCard)
            {
                if (currentTeam.WildCard <= 0)
                {
                    throw new Exception("You not have valid card!");
                }

                //if (_unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                //{
                //    Fk_AccountTeam = currentTeam.Id,
                //    GameWeakFrom = gameWeakFrom,
                //    GameWeakTo = gameWeakTo,
                //    WildCard = true
                //}, otherLang: false).Count() >= 1)
                //{
                //    throw new Exception("You already use card in this half of season!");
                //}
            }
            else if (cardTypeEnum == CardTypeEnum.DoubleGameWeak)
            {
                if (currentTeam.DoubleGameWeak <= 0)
                {
                    throw new Exception("You not have valid card!");
                }

                //if (_unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                //{
                //    Fk_AccountTeam = currentTeam.Id,
                //    GameWeakFrom = gameWeakFrom,
                //    GameWeakTo = gameWeakTo,
                //    DoubleGameWeak = true
                //}, otherLang: false).Count() >= 2)
                //{
                //    throw new Exception("You already use card in this half of season!");
                //}
            }
            else if (cardTypeEnum == CardTypeEnum.Top_11)
            {
                if (currentTeam.Top_11 <= 0)
                {
                    throw new Exception("You not have valid card!");
                }

                //if (_unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                //{
                //    Fk_AccountTeam = currentTeam.Id,
                //    GameWeakFrom = gameWeakFrom,
                //    GameWeakTo = gameWeakTo,
                //    Top_11 = true
                //}, otherLang: false).Count() >= 2)
                //{
                //    throw new Exception("You already use card in this half of season!");
                //}
            }
            else if (cardTypeEnum == CardTypeEnum.TripleCaptain)
            {
                if (currentTeam.TripleCaptain <= 0)
                {
                    throw new Exception("You not have valid card!");
                }

                //if (_unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                //{
                //    Fk_AccountTeam = currentTeam.Id,
                //    GameWeakFrom = gameWeakFrom,
                //    GameWeakTo = gameWeakTo,
                //    TripleCaptain = true
                //}, otherLang: false).Count() >= 2)
                //{
                //    throw new Exception("You already use card in this half of season!");
                //}
            }

            AccountTeamGameWeak accountTeamGameWeak = await _unitOfWork.AccountTeam.FindAccountTeamGameWeakbyId(teamGameWeak.Id, trackChanges: true);
            AccountTeam accountTeam = await _unitOfWork.AccountTeam.FindAccountTeambyId(currentTeam.Id, trackChanges: true);

            if (cardTypeEnum == CardTypeEnum.BenchBoost)
            {
                accountTeamGameWeak.BenchBoost = true;
                accountTeam.BenchBoost--;
            }
            else if (cardTypeEnum == CardTypeEnum.FreeHit)
            {
                accountTeamGameWeak.FreeHit = true;
                accountTeamGameWeak.TansfarePoints = 0;
                accountTeam.FreeTransfer = 0;
                accountTeam.FreeHit--;
            }
            else if (cardTypeEnum == CardTypeEnum.WildCard)
            {
                accountTeamGameWeak.WildCard = true;
                accountTeamGameWeak.TansfarePoints = 0;
                accountTeam.FreeTransfer = 0;
                accountTeam.WildCard--;
            }
            else if (cardTypeEnum == CardTypeEnum.DoubleGameWeak)
            {
                accountTeamGameWeak.DoubleGameWeak = true;
                accountTeam.DoubleGameWeak--;
            }
            else if (cardTypeEnum == CardTypeEnum.Top_11)
            {
                accountTeamGameWeak.Top_11 = true;
                accountTeam.Top_11--;
            }
            else if (cardTypeEnum == CardTypeEnum.TripleCaptain)
            {
                accountTeamGameWeak.TripleCaptain = true;
                accountTeam.TripleCaptain--;
            }

            await _unitOfWork.Save();

            return true;
        }

        [HttpPost]
        [Route(nameof(DeActivateCard))]
        public async Task<bool> DeActivateCard([FromQuery, BindRequired] CardTypeEnum cardTypeEnum)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
            _ = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SeasonModel currentSeason = _unitOfWork.Season.GetCurrentSeason();
            if (currentSeason == null)
            {
                throw new Exception("Season not started yet!");
            }

            GameWeakModel nextGameWeak = _unitOfWork.Season.GetNextGameWeak();
            if (nextGameWeak == null || nextGameWeak._365_GameWeakId_Parsed == null)
            {
                throw new Exception("Game Weak not started yet!");
            }

            AccountTeamModel currentTeam = _unitOfWork.AccountTeam.GetCurrentTeam(auth.Fk_Account, currentSeason.Id);
            if (currentTeam == null)
            {
                throw new Exception("Please create your team!");
            }

            AccountTeamGameWeakModel teamGameWeak = _unitOfWork.AccountTeam.GetTeamGameWeak(auth.Fk_Account, nextGameWeak.Id);
            if (currentTeam == null)
            {
                throw new Exception("Game Weak not started yet!");
            }

            AccountTeamGameWeak accountTeamGameWeak = await _unitOfWork.AccountTeam.FindAccountTeamGameWeakbyId(teamGameWeak.Id, trackChanges: true);
            AccountTeam accountTeam = await _unitOfWork.AccountTeam.FindAccountTeambyId(currentTeam.Id, trackChanges: true);

            if (cardTypeEnum == CardTypeEnum.BenchBoost && accountTeamGameWeak.BenchBoost == true)
            {
                accountTeamGameWeak.BenchBoost = false;
                accountTeam.BenchBoost++;
            }
            else if (cardTypeEnum == CardTypeEnum.DoubleGameWeak && accountTeamGameWeak.DoubleGameWeak == true)
            {
                accountTeamGameWeak.DoubleGameWeak = false;
                accountTeam.DoubleGameWeak++;
            }
            else if (cardTypeEnum == CardTypeEnum.TripleCaptain && accountTeamGameWeak.TripleCaptain == true)
            {
                accountTeamGameWeak.TripleCaptain = false;
                accountTeam.TripleCaptain++;
            }
            else if (cardTypeEnum == CardTypeEnum.Top_11 && accountTeamGameWeak.Top_11 == true)
            {
                accountTeamGameWeak.Top_11 = false;
                accountTeam.Top_11++;
            }

            await _unitOfWork.Save();

            return true;
        }
    }
}
