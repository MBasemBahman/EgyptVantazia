using Dashboard.Areas.AccountTeamEntity.Models;
using Dashboard.Areas.PlayerTransferEntity.Models;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PlayerTransfersModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;
namespace Dashboard.Areas.AccountTeamEntity.Controllers
{
    [Area("AccountTeamEntity")]
    [Authorize(DashboardViewEnum.AccountTeam, AccessLevelEnum.View)]
    public class AccountTeamController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public AccountTeamController(ILoggerManager logger, IMapper mapper,
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
            AccountTeamFilter filter = new()
            {
                Fk_Account = Fk_Account
            };

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            SetViewData(ProfileLayOut);
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] AccountTeamFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AccountTeamParameters parameters = new()
            {
                SearchColumns = "Id,FullName"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<AccountTeamModel> data = await _unitOfWork.AccountTeam.GetAccountTeamPaged(parameters, otherLang);

            List<AccountTeamDto> resultDto = _mapper.Map<List<AccountTeamDto>>(data);

            DataTable<AccountTeamDto> dataTableManager = new();

            DataTableResult<AccountTeamDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.AccountTeam.GetAccountTeamCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Profile(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AccountTeamDto data = _mapper.Map<AccountTeamDto>(_unitOfWork.AccountTeam
                                                           .GetAccountTeambyId(id, otherLang));


            data.AccountTeamGameWeaks = _mapper.Map<List<AccountTeamGameWeakDto>>
                (_unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
                {
                    Fk_AccountTeam = id
                },otherLang));

            data.PlayerTransfers = _mapper.Map<List<PlayerTransferDto>>
                (_unitOfWork.PlayerTransfers.GetPlayerTransfers(new PlayerTransferParameters
                {
                    Fk_AccountTeam = id
                },otherLang));

            data.AccountTeamPlayers = _mapper.Map<List<AccountTeamPlayerDto>>
                (_unitOfWork.AccountTeam.GetAccountTeamPlayers(new AccountTeamPlayerParameters
                {
                    Fk_AccountTeam = id
                },otherLang));

         
            return View(data);
        }

        [Authorize(DashboardViewEnum.AccountTeam, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int fk_Account, int id = 0)
        {
            AccountTeamCreateOrEditModel model = new()
            {
                Fk_Account = fk_Account
            };
            
            if (id > 0)
            {
                AccountTeam accountTeamDB = await _unitOfWork.AccountTeam.FindAccountTeambyId(id, trackChanges: false);
                model = _mapper.Map<AccountTeamCreateOrEditModel>(accountTeamDB);
                model.ImageUrl = accountTeamDB.StorageUrl + accountTeamDB.ImageUrl;
            }

            if (model.ImageUrl.IsNullOrEmpty())
            {
                model.ImageUrl = "calendar-date.png";
                model.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
            }
            
            SetViewData(ProfileLayOut: true);
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.AccountTeam, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, AccountTeamCreateOrEditModel model)
        {
            if (!ModelState.IsValid)
            {
                SetViewData(ProfileLayOut: false);

                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                AccountTeam accountTeamDB = new();
                if (id == 0)
                {
                    accountTeamDB = _mapper.Map<AccountTeam>(model);

                    accountTeamDB.CreatedBy = auth.UserName;
                    
                    _unitOfWork.AccountTeam.CreateAccountTeam(accountTeamDB);

                }
                else
                {
                    accountTeamDB = await _unitOfWork.AccountTeam.FindAccountTeambyId(id, trackChanges: true);

                    _ = _mapper.Map(model, accountTeamDB);

                    accountTeamDB.LastModifiedBy = auth.UserName;
                }

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];

                if (imageFile != null)
                {
                    accountTeamDB.ImageUrl = await _unitOfWork.AccountTeam.UploadAccountTeamImage(_environment.WebRootPath, imageFile);
                    accountTeamDB.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
                }

                await _unitOfWork.Save();

                return RedirectToAction("Profile", "Account", new {area = "AccountEntity", id = model.Fk_Account});
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(ProfileLayOut: false);

            return View(model);
        }

        // helper methods
        private void SetViewData(bool ProfileLayOut = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);
            ViewData["Player"] = _unitOfWork.Team.GetPlayerLookUp(new PlayerParameters(), otherLang);
            ViewData["ProfileLayOut"] = ProfileLayOut;
        }

    }
}
