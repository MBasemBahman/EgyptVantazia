using Dashboard.Areas.PlayerStateEntity.Models;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerStateModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.PlayerStateEntity.Controllers
{
    [Area("PlayerStateEntity")]
    [Authorize(DashboardViewEnum.PlayerSeasonScoreState, AccessLevelEnum.View)]
    public class PlayerSeasonScoreStateController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public PlayerSeasonScoreStateController(ILoggerManager logger, IMapper mapper,
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
        public IActionResult Index(int id)
        {
            PlayerSeasonScoreStateFilter filter = new()
            {
                Id = id
            };

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            return View(filter);
        }
        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] PlayerSeasonScoreStateFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerSeasonScoreStateParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<PlayerSeasonScoreStateModel> data = await _unitOfWork.PlayerState.GetPlayerSeasonScoreStatePaged(parameters, otherLang);

            List<PlayerSeasonScoreStateDto> resultDto = _mapper.Map<List<PlayerSeasonScoreStateDto>>(data);

            DataTable<PlayerSeasonScoreStateDto> dataTableManager = new();

            DataTableResult<PlayerSeasonScoreStateDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.PlayerState.GetPlayerSeasonScoreStateCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerSeasonScoreStateDto data = _mapper.Map<PlayerSeasonScoreStateDto>(_unitOfWork.PlayerState
                                                            .GetPlayerSeasonScoreStatebyId(id, otherLang));

            return View(data);
        }

        [Authorize(DashboardViewEnum.PlayerSeasonScoreState, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            PlayerSeasonScoreStateCreateOrEditModel model = new();

            if (id > 0)
            {
                model = _mapper.Map<PlayerSeasonScoreStateCreateOrEditModel>(
                                                await _unitOfWork.PlayerState.FindPlayerSeasonScoreStatebyId(id, trackChanges: false));
            }

            SetViewDataValues();
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.PlayerSeasonScoreState, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, PlayerSeasonScoreStateCreateOrEditModel model)
        {
            if (!ModelState.IsValid)
            {
                SetViewDataValues();
                
                return View(model);
            }
            try
            {
                PlayerSeasonScoreState dataDb = new();

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

                if (id == 0)
                {

                    dataDb = _mapper.Map<PlayerSeasonScoreState>(model);

                    dataDb.CreatedBy = auth.UserName;

                    _unitOfWork.PlayerState.CreatePlayerSeasonScoreState(dataDb);
                }
                else
                {
                    dataDb = await _unitOfWork.PlayerState.FindPlayerSeasonScoreStatebyId(id, trackChanges: true);

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

        [Authorize(DashboardViewEnum.PlayerSeasonScoreState, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            PlayerSeasonScoreState data = await _unitOfWork.PlayerState.FindPlayerSeasonScoreStatebyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.PlayerSeasonScoreState, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.PlayerState.DeletePlayerSeasonScoreState(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public void SetViewDataValues()
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            
            ViewData["ScoreState"] = _unitOfWork.PlayerState.GetScoreStatesLookUp(new ScoreStateParameters(), otherLang);
            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);
            ViewData["GameWeak"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters(), otherLang);
            ViewData["Player"] = _unitOfWork.Team.GetPlayerLookUp(new PlayerParameters(), otherLang);
        }
    }
}
