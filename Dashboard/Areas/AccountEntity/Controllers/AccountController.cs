using Dashboard.Areas.AccountEntity.Models;
using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.SubscriptionModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.CoreServicesModels.UserModels;
using Entities.DBModels.AccountModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.AccountEntity.Controllers
{
    [Area("AccountEntity")]
    [Authorize(DashboardViewEnum.Account, AccessLevelEnum.View)]
    public class AccountController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public AccountController(ILoggerManager logger, IMapper mapper,
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

        public IActionResult Index()
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AccountFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            SetViewData(IsProfile: false, id: 0, otherLang);

            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] AccountFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AccountParameters parameters = new()
            {
                SearchColumns = "Id,UserName,FullName"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<AccountModel> data = await _unitOfWork.Account.GetAccountsPaged(parameters, otherLang);

            List<AccountDto> resultDto = _mapper.Map<List<AccountDto>>(data);

            DataTable<AccountDto> dataTableManager = new();

            DataTableResult<AccountDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Account.GetAccountsCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AccountDto data = _mapper.Map<AccountDto>(_unitOfWork.Account
                                                           .GetAccountbyId(id, otherLang));

            return View(data);
        }

        public IActionResult Profile(int id, int returnItem = (int)AccountProfileItems.Details)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AccountDto data = _mapper.Map<AccountDto>(_unitOfWork.Account
                                                           .GetAccountbyId(id, otherLang));

            ViewData["returnItem"] = returnItem;
            ViewData["otherLang"] = otherLang;


            return View(data);
        }

        [Authorize(DashboardViewEnum.Account, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0, bool IsProfile = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            UserAccountCreateOrEditModel model = new();

            if (id > 0)
            {
                Account accountDB = await _unitOfWork.Account.FindAccountById(id, trackChanges: false);
                model.Account = _mapper.Map<AccountCreateModel>(accountDB);
                model.User = _mapper.Map<UserCreateModel>(await _unitOfWork.User.FindByAccountId(id, trackChanges: false));
                model.ImageUrl = accountDB.StorageUrl + accountDB.ImageUrl;
                model.Subscriptions = _mapper.Map<List<AccountSubscriptionModel>>(_unitOfWork
                    .AccountSubscription.GetAccountSubscriptions(new AccountSubscriptionParameters
                    { Fk_Account = accountDB.Id }, otherLang).ToList());
            }

            SetViewData(IsProfile, id, otherLang);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.Account, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, UserAccountCreateOrEditModel model, bool IsProfile = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (!ModelState.IsValid)
            {
                SetViewData(IsProfile, id, otherLang);

                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                Account accountDB = new();
                if (id == 0)
                {
                    accountDB = _mapper.Map<Account>(model.Account);

                    accountDB.CreatedBy = auth.UserName;

                    accountDB.User = new();

                    _ = _mapper.Map(model.User, accountDB.User);
                    accountDB.User.CreatedBy = auth.UserName;

                    _unitOfWork.Account.CreateAccount(accountDB);

                }
                else
                {
                    accountDB = await _unitOfWork.Account.FindAccountById(id, trackChanges: true);
                    User userDB = await _unitOfWork.User.FindByAccountId(id, trackChanges: true);

                    if (model.User.Password != userDB.Password)
                    {
                        model.User.Password = _unitOfWork.User.ChangePassword(model.User.Password);
                    }

                    _ = _mapper.Map(model.User, userDB);
                    _ = _mapper.Map(model.Account, accountDB);

                    userDB.LastModifiedBy = auth.UserName;
                }

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];

                if (imageFile != null)
                {
                    accountDB.ImageUrl = await _unitOfWork.Account.UploudAccountImage(_environment.WebRootPath, imageFile);
                    accountDB.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
                }

                await _unitOfWork.Save();

                _unitOfWork.Account.UpdateAccountSubscriptions(accountDB.Id, model.Subscriptions);

                await _unitOfWork.Save();

                return IsProfile ? RedirectToAction(nameof(Profile), new { id }) : (IActionResult)RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(IsProfile, id, otherLang);

            return View(model);
        }

        [Authorize(DashboardViewEnum.Account, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            Account data = await _unitOfWork.Account.FindAccountById(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.Account, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Account.DeleteAccount(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(bool IsProfile, int id, bool otherLang)
        {
            ViewData["IsProfile"] = IsProfile;
            ViewData["id"] = id;
            ViewData["Countrys"] = _unitOfWork.Location.GetCountrysLookUp(new RequestParameters(), otherLang);
            ViewData["Teams"] = _unitOfWork.Team.GetTeams(new TeamParameters(), otherLang)
                .ToDictionary(a => a.Id.ToString(), a => a.Name);
            ViewData["Subscription"] = _unitOfWork.Subscription.GetSubscriptionsLookUp(new SubscriptionParameters(), otherLang);
        }


    }
}
