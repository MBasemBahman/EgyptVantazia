using Dashboard.Areas.AccountTeamGameWeakEntity.Models;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.RequestFeatures;
namespace Dashboard.Areas.AccountTeamGameWeakEntity.Controllers
{
    [Area("AccountTeamGameWeakEntity")]
    [Authorize(DashboardViewEnum.AccountTeamGameWeak, AccessLevelEnum.View)]
    public class AccountTeamGameWeakController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public AccountTeamGameWeakController(ILoggerManager logger, IMapper mapper,
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
            AccountTeamGameWeakFilter filter = new()
            {
                Fk_Account = Fk_Account
            };

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            SetViewData(ProfileLayOut);
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] AccountTeamGameWeakFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AccountTeamGameWeakParameters parameters = new()
            {
                SearchColumns = ""
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<AccountTeamGameWeakModel> data = await _unitOfWork.AccountTeam.GetAccountTeamGameWeakPaged(parameters, otherLang);

            List<AccountTeamGameWeakDto> resultDto = _mapper.Map<List<AccountTeamGameWeakDto>>(data);

            DataTable<AccountTeamGameWeakDto> dataTableManager = new();

            DataTableResult<AccountTeamGameWeakDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.AccountTeam.GetAccountTeamGameWeakCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        // public IActionResult Profile(int id)
        // {
        //     bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
        //
        //     AccountTeamGameWeakDto data = _mapper.Map<AccountTeamGameWeakDto>(_unitOfWork.AccountTeam
        //                                                    .GetAccountTeamGameWeakbyId(id, otherLang));
        //
        //
        //     data.AccountTeamGameWeakGameWeaks = _mapper.Map<List<AccountTeamGameWeakDto>>
        //         (_unitOfWork.AccountTeam.GetAccountTeamGameWeaks(new AccountTeamGameWeakParameters
        //         {
        //             Fk_AccountTeam = id
        //         }, otherLang));
        //
        //     data.PlayerTransfers = _mapper.Map<List<PlayerTransferDto>>
        //         (_unitOfWork.PlayerTransfers.GetPlayerTransfers(new PlayerTransferParameters
        //         {
        //             Fk_AccountTeamGameWeak = id
        //         }, otherLang));
        //
        //     data.AccountTeamGameWeakPlayers = _mapper.Map<List<AccountTeamGameWeakPlayerDto>>
        //         (_unitOfWork.AccountTeam.GetAccountTeamGameWeakPlayers(new AccountTeamGameWeakPlayerParameters
        //         {
        //             Fk_AccountTeamGameWeak = id
        //         }, otherLang));
        //
        //
        //     return View(data);
        // }

        // helper methods
        private void SetViewData(bool ProfileLayOut = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);
            ViewData["Player"] = _unitOfWork.Team.GetPlayerLookUp(new PlayerParameters(), otherLang);
            ViewData["GameWeak"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters(), otherLang);
            ViewData["ProfileLayOut"] = ProfileLayOut;
        }

    }
}
