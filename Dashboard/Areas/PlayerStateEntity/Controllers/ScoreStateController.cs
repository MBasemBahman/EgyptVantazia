using Dashboard.Areas.PlayerStateEntity.Models;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.DBModels.PlayerStateModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.PlayerStateEntity.Controllers
{
    [Area("PlayerStateEntity")]
    [Authorize(DashboardViewEnum.ScoreState, AccessLevelEnum.View)]
    public class ScoreStateController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public ScoreStateController(ILoggerManager logger, IMapper mapper,
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
            ScoreStateFilter filter = new()
            {
                Id = id
            };

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            return View(filter);
        }
        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] ScoreStateFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ScoreStateParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<ScoreStateModel> data = await _unitOfWork.PlayerState.GetScoreStatePaged(parameters, otherLang);

            List<ScoreStateDto> resultDto = _mapper.Map<List<ScoreStateDto>>(data);

            DataTable<ScoreStateDto> dataTableManager = new();

            DataTableResult<ScoreStateDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.PlayerState.GetScoreStateCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ScoreStateDto data = _mapper.Map<ScoreStateDto>(_unitOfWork.PlayerState
                                                            .GetScoreStatebyId(id, otherLang));

            return View(data);
        }

        [Authorize(DashboardViewEnum.ScoreState, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            ScoreStateCreateOrEditModel model = new()
            {
                ScoreStateLang = new(),
            };

            if (id > 0)
            {
                model = _mapper.Map<ScoreStateCreateOrEditModel>(
                                                await _unitOfWork.PlayerState.FindScoreStatebyId(id, trackChanges: false));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.ScoreState, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, ScoreStateCreateOrEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                ScoreState dataDb = new();

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

                if (id == 0)
                {

                    dataDb = _mapper.Map<ScoreState>(model);

                    dataDb.CreatedBy = auth.UserName;

                    _unitOfWork.PlayerState.CreateScoreState(dataDb);
                }
                else
                {
                    dataDb = await _unitOfWork.PlayerState.FindScoreStatebyId(id, trackChanges: true);

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

        [Authorize(DashboardViewEnum.ScoreState, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            ScoreState data = await _unitOfWork.PlayerState.FindScoreStatebyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.ScoreState, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.PlayerState.DeleteScoreState(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
