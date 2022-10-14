using Dashboard.Areas.AccountTeamEntity.Models;
using Dashboard.Areas.PlayerTransferEntity.Models;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PlayerTransfersModels;
using Entities.CoreServicesModels.SeasonModels;
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
                SearchColumns = "Id"
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



        // helper methods
        private void SetViewData(bool ProfileLayOut = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);
            ViewData["ProfileLayOut"] = ProfileLayOut;
        }

    }
}
