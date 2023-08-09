using Dashboard.Areas.AccountTeamEntity.Models;
using Dashboard.Areas.PlayerTransferEntity.Models;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PlayerTransfersModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;
using PlayerTransferDto = Dashboard.Areas.PlayerTransferEntity.Models.PlayerTransferDto;

namespace Dashboard.Areas.AccountTeamEntity.Controllers
{
    [Area("AccountTeamEntity")]
    [Authorize(DashboardViewEnum.AccountTeam, AccessLevelEnum.View)]
    public class PlayerTransferController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;
        private readonly UpdateResultsUtils _updateResultsUtils;

        public PlayerTransferController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork,
                 LinkGenerator linkGenerator,
                 IWebHostEnvironment environment, UpdateResultsUtils updateResultsUtils)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _linkGenerator = linkGenerator;
            _environment = environment;
            _updateResultsUtils = updateResultsUtils;
        }

        public IActionResult Index(int Fk_Account, int Fk_Player, int Fk_Team, bool ProfileLayOut = false)
        {
            PlayerTransferFilter filter = new()
            {
                Fk_Account = Fk_Account,
                Fk_Player = Fk_Player,
                Fk_Team = Fk_Team
            };

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            
            SetViewData(ProfileLayOut);
            
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] PlayerTransferFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerTransferParameters parameters = new()
            {
                SearchColumns = ""
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<PlayerTransferModel> data = await _unitOfWork.PlayerTransfers.GetPlayerTransferPaged(parameters, otherLang);

            List<PlayerTransferDto> resultDto = _mapper.Map<List<PlayerTransferDto>>(data);

            DataTable<PlayerTransferDto> dataTableManager = new();

            DataTableResult<PlayerTransferDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.PlayerTransfers.GetPlayerTransferCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }
        
        // helper methods
        private void SetViewData(bool ProfileLayOut = false, int fk_Season = 0)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);
            ViewData["Player"] = _unitOfWork.Team.GetPlayerLookUp(new PlayerParameters(), otherLang);
            ViewData["GameWeak"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters(), otherLang);
            ViewData["Team"] = _unitOfWork.Team.GetTeamLookUp(new TeamParameters
            {
                Fk_Season = fk_Season
            }, otherLang);
            ViewData["ProfileLayOut"] = ProfileLayOut;
        }

    }
}
