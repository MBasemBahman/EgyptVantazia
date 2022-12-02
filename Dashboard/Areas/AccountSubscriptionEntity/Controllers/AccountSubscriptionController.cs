using Dashboard.Areas.AccountEntity.Models;
using Dashboard.Areas.AccountSubscriptionEntity.Models;
using Dashboard.Areas.PlayerTransferEntity.Models;
using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.PlayerTransfersModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.SubscriptionModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountModels;
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

        public IActionResult Index(int Fk_Account, bool ProfileLayOut = false)
        {
            AccountSubscriptionFilter filter = new()
            {
                Fk_Account = Fk_Account
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
                SearchColumns = "Id,FullName"
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

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                AccountSubscription accountSubscriptionDB = new();
                if (id == 0)
                {
                    accountSubscriptionDB = _mapper.Map<AccountSubscription>(model);

                    _unitOfWork.Account.CreateAccountSubscription(accountSubscriptionDB);

                }
                else
                {
                    accountSubscriptionDB = await _unitOfWork.Account.FindAccountSubscriptionById(id, trackChanges: true);

                    _ = _mapper.Map(model, accountSubscriptionDB);
                }

                await _unitOfWork.Save();

                if (returnPage == (int)AccountSubscriptionReturnPageEnum.Index)
                {
                    return RedirectToAction("Index", "AccountSubscription", new
                    {
                        area = "AccountSubscriptionEntity"
                    });
                }
                
                return RedirectToAction("Profile", "Account", new
                {
                    area = "AccountEntity", id = model.Fk_Account,
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
