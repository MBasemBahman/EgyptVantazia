using Dashboard.Areas.PlayerScoreEntity.Models;
using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.RequestFeatures;
using Org.BouncyCastle.Ocsp;

namespace Dashboard.Areas.PlayerScoreEntity.Controllers
{
    [Area("PlayerScoreEntity")]
    [Authorize(DashboardViewEnum.ScoreType, AccessLevelEnum.View)]
    public class ScoreTypeController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public ScoreTypeController(ILoggerManager logger, IMapper mapper,
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
            ScoreTypeFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] ScoreTypeFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ScoreTypeParameters parameters = new()
            {
                SearchColumns = "Id,Name",
                IncludeBestPlayer = true
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<ScoreTypeModel> data = await _unitOfWork.PlayerScore.GetScoreTypePaged(parameters, otherLang);

            List<ScoreTypeDto> resultDto = _mapper.Map<List<ScoreTypeDto>>(data);

            DataTable<ScoreTypeDto> dataTableManager = new();

            DataTableResult<ScoreTypeDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.PlayerScore.GetScoreTypeCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ScoreTypeDto data = _mapper.Map<ScoreTypeDto>(_unitOfWork.PlayerScore
                                                           .GetScoreTypebyId(id, otherLang));

            return View(data);
        }


        [Authorize(DashboardViewEnum.ScoreType, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            ScoreTypeCreateOrEditModel model = new()
            {
                ScoreTypeLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<ScoreTypeCreateOrEditModel>(
                                                await _unitOfWork.PlayerScore.FindScoreTypebyId(id, trackChanges: false));
            }

            SetViewData();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.ScoreType, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, ScoreTypeCreateOrEditModel model)
        {

            if (!ModelState.IsValid)
            {
                SetViewData();
                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                ScoreType dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<ScoreType>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.PlayerScore.CreateScoreType(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.PlayerScore.FindScoreTypebyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;
                }


                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData();
            return View(model);
        }

        [Authorize(DashboardViewEnum.ScoreType, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            ScoreType data = await _unitOfWork.PlayerScore.FindScoreTypebyId(id, trackChanges: false);

            return View(data != null && !_unitOfWork.PlayerScore.GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters
            {
                Fk_ScoreType = id
            }, otherLang: false).Any());
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.ScoreType, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.PlayerScore.DeleteScoreType(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }


        public void SetViewData()
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            
            ViewData["Players"] = _unitOfWork.Team.GetPlayerLookUp(new PlayerParameters(), otherLang);
        }
    }
}
