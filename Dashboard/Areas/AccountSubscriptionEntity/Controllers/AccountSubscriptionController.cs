using Dashboard.Areas.AccountEntity.Models;
using Dashboard.Areas.AccountSubscriptionEntity.Models;
using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.SubscriptionModels;
using Entities.DBModels.AccountModels;
using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;
namespace Dashboard.Areas.AccountSubscriptionEntity.Controllers
{
    [Area("AccountSubscriptionEntity")]
    [Authorize(DashboardViewEnum.AccountSubscription, AccessLevelEnum.View)]
    public class AccountSubscriptionController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public AccountSubscriptionController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork,
                 LinkGenerator linkGenerator,
                 IWebHostEnvironment environment)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _linkGenerator = linkGenerator;
            _environment = environment;
        }

        public IActionResult Index(int Fk_Account, int Fk_Subscription, bool ProfileLayOut = false)
        {
            AccountSubscriptionFilter filter = new()
            {
                Fk_Account = Fk_Account,
                Fk_Subscription = Fk_Subscription
            };

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            SetViewData(ProfileLayOut);
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] AccountSubscriptionFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AccountSubscriptionParameters parameters = new()
            {
                SearchColumns = ""
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<AccountSubscriptionModel> data = await _unitOfWork.Account.GetAccountSubscriptionsPaged(parameters, otherLang);

            List<AccountSubscriptionDto> resultDto = _mapper.Map<List<AccountSubscriptionDto>>(data);

            DataTable<AccountSubscriptionDto> dataTableManager = new();

            DataTableResult<AccountSubscriptionDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Account.GetAccountSubscriptionsCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        [Authorize(DashboardViewEnum.AccountSubscription, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int fk_Account, int id = 0,
            int returnPage = (int)AccountSubscriptionReturnPageEnum.Index)
        {
            AccountSubscriptionCreateOrEditModel model = new()
            {
                Fk_Account = fk_Account
            };

            if (id > 0)
            {
                AccountSubscription accountSubscriptionDB = await _unitOfWork.Account.FindAccountSubscriptionById(id, trackChanges: false);
                model = _mapper.Map<AccountSubscriptionCreateOrEditModel>(accountSubscriptionDB);
            }

            SetViewData(ProfileLayOut: true);
            ViewData["returnPage"] = returnPage;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.AccountSubscription, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, AccountSubscriptionCreateOrEditModel model,
            int returnPage = (int)AccountSubscriptionReturnPageEnum.Index)
        {
            if (!ModelState.IsValid)
            {
                SetViewData(ProfileLayOut: false);
                ViewData["returnPage"] = returnPage;

                return View(model);
            }
            try
            {
                if (model.IsActive)
                {
                    SubscriptionModel prevSubscription = _unitOfWork.Subscription.GetSubscriptions(new SubscriptionParameters
                    {
                        Fk_Account = model.Fk_Account,
                        Fk_Season = model.Fk_Season,
                        Id = model.Fk_Subscription,
                    }, otherLang: false).FirstOrDefault();

                    if (prevSubscription != null && !prevSubscription.IsValid)
                    {
                        throw new Exception("Account already get this subscription in this season!");
                    }
                }

                //if (model.Fk_Subscription == (int)SubscriptionEnum.All)
                //{
                //    if (_unitOfWork.Account.GetAccountSubscriptions(new AccountSubscriptionParameters
                //    {
                //        Fk_Account = model.Fk_Account,
                //        Fk_Season = model.Fk_Season,
                //        //NotEqualSubscriptionId = (int)SubscriptionEnum.Add3MillionsBank
                //    }, otherLang: false).Any())
                //    {
                //        throw new Exception("Account can`t buy this subscription because you already have sub one in this season!");
                //    }
                //}
                //else if (model.Fk_Subscription != (int)SubscriptionEnum.All)
                //{
                //    if (_unitOfWork.Account.GetAccountSubscriptions(new AccountSubscriptionParameters
                //    {
                //        Fk_Account = model.Fk_Account,
                //        Fk_Season = model.Fk_Season,
                //        Fk_Subscription = (int)SubscriptionEnum.All
                //    }, otherLang: false).Any())
                //    {
                //        throw new Exception("Account can`t buy this subscription because you already have super one in this season!");
                //    }
                //}

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                AccountSubscription accountSubscriptionDB = new();
                if (id == 0)
                {
                    accountSubscriptionDB = _mapper.Map<AccountSubscription>(model);

                    _unitOfWork.Account.CreateAccountSubscription(accountSubscriptionDB);

                    AccountTeamModel currentTeam = _unitOfWork.AccountTeam.GetCurrentTeam(model.Fk_Account, model.Fk_Season);
                    AccountTeam accounTeam = await _unitOfWork.AccountTeam.FindAccountTeambyId(currentTeam.Id, trackChanges: true);

                    if (accountSubscriptionDB.Fk_Subscription == (int)SubscriptionEnum.Gold)
                    {
                        accounTeam.TripleCaptain++;
                        accounTeam.DoubleGameWeak++;
                        accounTeam.TwiceCaptain++;
                        accounTeam.BenchBoost++;
                        accounTeam.Top_11++;
                        accounTeam.FreeHit++;
                        accounTeam.WildCard++;
                        accounTeam.IsVip = true;
                        accounTeam.TotalMoney += 3;

                        Account account = await _unitOfWork.Account.FindAccountById(accounTeam.Fk_Account, trackChanges: true);
                        account.ShowAds = false;
                    }
                    else if (accountSubscriptionDB.Fk_Subscription == (int)SubscriptionEnum.TripleCaptain)
                    {
                        accounTeam.TripleCaptain++;
                    }
                    else if (accountSubscriptionDB.Fk_Subscription == (int)SubscriptionEnum.DoubleGameWeak)
                    {
                        accounTeam.DoubleGameWeak++;
                    }
                    else if (accountSubscriptionDB.Fk_Subscription == (int)SubscriptionEnum.BenchBoost)
                    {
                        accounTeam.BenchBoost++;
                    }
                    else if (accountSubscriptionDB.Fk_Subscription == (int)SubscriptionEnum.Top_11)
                    {
                        accounTeam.Top_11++;
                    }
                    else if (accountSubscriptionDB.Fk_Subscription == (int)SubscriptionEnum.TwiceCaptain)
                    {
                        accounTeam.TwiceCaptain++;
                    }
                    else if (accountSubscriptionDB.Fk_Subscription == (int)SubscriptionEnum.FreeHit)
                    {
                        accounTeam.FreeHit++;
                    }
                    else if (accountSubscriptionDB.Fk_Subscription == (int)SubscriptionEnum.WildCard)
                    {
                        accounTeam.WildCard++;
                    }
                    else if (accountSubscriptionDB.Fk_Subscription == (int)SubscriptionEnum.Add3MillionsBank)
                    {
                        accounTeam.TotalMoney += 3;
                    }
                    else if (accountSubscriptionDB.Fk_Subscription == (int)SubscriptionEnum.RemoveAds)
                    {
                        Account account = await _unitOfWork.Account.FindAccountById(accounTeam.Fk_Account, trackChanges: true);
                        account.ShowAds = false;
                    }
                }
                else
                {
                    accountSubscriptionDB = await _unitOfWork.Account.FindAccountSubscriptionById(id, trackChanges: true);

                    accountSubscriptionDB.IsActive = model.IsActive;
                }

                await _unitOfWork.Save();


                return returnPage == (int)AccountSubscriptionReturnPageEnum.Index
                    ? RedirectToAction("Index", "AccountSubscription", new
                    {
                        area = "AccountSubscriptionEntity"
                    })
                    : (IActionResult)RedirectToAction("Profile", "Account", new
                    {
                        area = "AccountEntity",
                        id = model.Fk_Account,
                        returnItem = (int)AccountProfileItems.AccountSubscription
                    });
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(ProfileLayOut: false);
            ViewData["returnPage"] = returnPage;

            return View(model);
        }

        // helper methods
        private void SetViewData(bool ProfileLayOut = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);
            ViewData["Account"] = _unitOfWork.Account.GetAccountLookUp(new AccountParameters(), otherLang);
            ViewData["Subscription"] = _unitOfWork.Subscription.GetSubscriptionsLookUp(new SubscriptionParameters(), otherLang);
            ViewData["ProfileLayOut"] = ProfileLayOut;
        }

    }
}
