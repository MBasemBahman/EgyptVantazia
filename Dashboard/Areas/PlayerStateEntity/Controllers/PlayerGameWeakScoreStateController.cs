using Dashboard.Areas.PlayerStateEntity.Models;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerStateModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.PlayerStateEntity.Controllers
{
    [Area("PlayerStateEntity")]
    [Authorize(DashboardViewEnum.PlayerGameWeakScoreState, AccessLevelEnum.View)]
    public class PlayerGameWeakScoreStateController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public PlayerGameWeakScoreStateController(ILoggerManager logger, IMapper mapper,
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
        public IActionResult Index(int id, int fk_Player, bool ProfileLayOut = false)
        {
            PlayerGameWeakScoreStateFilter filter = new()
            {
                Id = id,
                Fk_Player = fk_Player
            };

            ViewData["ProfileLayOut"] = ProfileLayOut;
            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            SetViewDataValues();
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] PlayerGameWeakScoreStateFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerGameWeakScoreStateParameters parameters = new()
            {
                SearchColumns = ""
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<PlayerGameWeakScoreStateModel> data = await _unitOfWork.PlayerState.GetPlayerGameWeakScoreStatePaged(parameters, otherLang);

            List<PlayerGameWeakScoreStateDto> resultDto = _mapper.Map<List<PlayerGameWeakScoreStateDto>>(data);

            DataTable<PlayerGameWeakScoreStateDto> dataTableManager = new();

            DataTableResult<PlayerGameWeakScoreStateDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.PlayerState.GetPlayerGameWeakScoreStateCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerGameWeakScoreStateDto data = _mapper.Map<PlayerGameWeakScoreStateDto>(_unitOfWork.PlayerState
                                                            .GetPlayerGameWeakScoreStatebyId(id, otherLang));

            return View(data);
        }

        [Authorize(DashboardViewEnum.PlayerGameWeakScoreState, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            PlayerGameWeakScoreStateCreateOrEditModel model = new();
            _ = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (id > 0)
            {
                model = _mapper.Map<PlayerGameWeakScoreStateCreateOrEditModel>(
                                                await _unitOfWork.PlayerState.FindPlayerGameWeakScoreStatebyId(id, trackChanges: false));
            }

            SetViewDataValues();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.PlayerGameWeakScoreState, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, PlayerGameWeakScoreStateCreateOrEditModel model)
        {
            if (!ModelState.IsValid)
            {
                SetViewDataValues();

                return View(model);
            }
            try
            {
                PlayerGameWeakScoreState dataDb = new();

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

                if (id == 0)
                {

                    dataDb = _mapper.Map<PlayerGameWeakScoreState>(model);

                    dataDb.CreatedBy = auth.UserName;

                    _unitOfWork.PlayerState.CreatePlayerGameWeakScoreState(dataDb);
                }
                else
                {
                    dataDb = await _unitOfWork.PlayerState.FindPlayerGameWeakScoreStatebyId(id, trackChanges: true);

                    dataDb.LastModifiedBy = auth.UserName;

                    _ = _mapper.Map(model, dataDb);
                }

                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }



            return View(model);
        }

        [Authorize(DashboardViewEnum.PlayerGameWeakScoreState, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            PlayerGameWeakScoreState data = await _unitOfWork.PlayerState.FindPlayerGameWeakScoreStatebyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.PlayerGameWeakScoreState, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.PlayerState.DeletePlayerGameWeakScoreState(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public void SetViewDataValues()
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ViewData["ScoreState"] = _unitOfWork.PlayerState.GetScoreStatesLookUp(new ScoreStateParameters(), otherLang);
            ViewData["GameWeak"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters(), otherLang);
            ViewData["Player"] = _unitOfWork.Team.GetPlayerLookUp(new PlayerParameters(), otherLang);
        }
    }
}
