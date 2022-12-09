using Dashboard.Areas.PlayerScoreEntity.Models;
using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.PlayerScoreEntity.Controllers
{
    [Area("PlayerScoreEntity")]
    [Authorize(DashboardViewEnum.PlayerGameWeakScore, AccessLevelEnum.View)]
    public class PlayerGameWeakScoreController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public PlayerGameWeakScoreController(ILoggerManager logger, IMapper mapper,
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

        public IActionResult Index(int Fk_Player, bool ProfileLayOut = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerGameWeakScoreFilter filter = new()
            {
                Fk_Player = Fk_Player
            };

            ViewData["ProfileLayOut"] = ProfileLayOut;
            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            ViewData["Players"] = _unitOfWork.Team.GetPlayerLookUp(new PlayerParameters(), otherLang);
            ViewData["Team"] = _unitOfWork.Team.GetTeamLookUp(new TeamParameters(), otherLang);
            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);
            ViewData["GameWeak"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters(), otherLang);
            ViewData["ScoreType"] = _unitOfWork.PlayerScore.GetScoreTypesLookUp(new ScoreTypeParameters(), otherLang);
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] PlayerGameWeakScoreFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerGameWeakScoreParameters parameters = new()
            {
                SearchColumns = ""
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<PlayerGameWeakScoreModel> data = await _unitOfWork.PlayerScore.GetPlayerGameWeakScorePaged(parameters, otherLang);

            List<PlayerGameWeakScoreDto> resultDto = _mapper.Map<List<PlayerGameWeakScoreDto>>(data);

            DataTable<PlayerGameWeakScoreDto> dataTableManager = new();

            DataTableResult<PlayerGameWeakScoreDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.PlayerScore.GetPlayerGameWeakScoreCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerGameWeakScoreDto data = _mapper.Map<PlayerGameWeakScoreDto>(_unitOfWork.PlayerScore
                                                           .GetPlayerGameWeakScorebyId(id, otherLang));

            return View(data);
        }

        [Authorize(DashboardViewEnum.PlayerGameWeakScore, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            PlayerGameWeakScore data = await _unitOfWork.PlayerScore.FindPlayerGameWeakScorebyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.PlayerGameWeakScore, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.PlayerScore.DeletePlayerGameWeakScore(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(int fk_Season, bool otherLang)
        {

            ViewData["GameWeak"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters()
            {
                Fk_Season = fk_Season
            }, otherLang);
            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);

            ViewData["ScoreType"] = _unitOfWork.PlayerScore.GetScoreTypesLookUp(new ScoreTypeParameters(), otherLang);


        }


    }
}
