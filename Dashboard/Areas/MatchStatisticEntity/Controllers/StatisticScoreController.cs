using Dashboard.Areas.MatchStatisticEntity.Models;
using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.MatchStatisticModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.MatchStatisticEntity.Controllers
{
    [Area("MatchStatisticEntity")]
    [Authorize(DashboardViewEnum.StatisticScore, AccessLevelEnum.View)]
    public class StatisticScoreController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public StatisticScoreController(ILoggerManager logger, IMapper mapper,
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
            StatisticScoreFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] StatisticScoreFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            StatisticScoreParameters parameters = new()
            {
                SearchColumns = "Id,Name",
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<StatisticScoreModel> data = await _unitOfWork.MatchStatistic.GetStatisticScoresPaged(parameters, otherLang);

            List<StatisticScoreDto> resultDto = _mapper.Map<List<StatisticScoreDto>>(data);

            DataTable<StatisticScoreDto> dataTableManager = new();

            DataTableResult<StatisticScoreDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.MatchStatistic.GetStatisticScoreCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            StatisticScoreDto data = _mapper.Map<StatisticScoreDto>(_unitOfWork.MatchStatistic
                                                           .GetStatisticScorebyId(id, otherLang));

            return View(data);
        }


        [Authorize(DashboardViewEnum.StatisticScore, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            StatisticScoreCreateOrEditModel model = new()
            {
                StatisticScoreLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<StatisticScoreCreateOrEditModel>(
                                                await _unitOfWork.MatchStatistic.FindStatisticScorebyId(id, trackChanges: false));
            }

            SetViewData();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.StatisticScore, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, StatisticScoreCreateOrEditModel model)
        {

            if (!ModelState.IsValid)
            {
                SetViewData();
                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                StatisticScore dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<StatisticScore>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.MatchStatistic.CreateStatisticScore(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.MatchStatistic.FindStatisticScorebyId(id, trackChanges: true);

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

        [Authorize(DashboardViewEnum.StatisticScore, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            StatisticScore data = await _unitOfWork.MatchStatistic.FindStatisticScorebyId(id, trackChanges: false);

            return View(data != null && !_unitOfWork.MatchStatistic.GetMatchStatisticScores(new MatchStatisticScoreParameters
            {
                Fk_StatisticScores = new List<int> { id}
            }, otherLang: false).Any());
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.StatisticScore, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.MatchStatistic.DeleteStatisticScore(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }


        public void SetViewData()
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ViewData["StatisticCategory"] = _unitOfWork.MatchStatistic.GetStatisticCategorysLookUp(new StatisticCategoryParameters(), otherLang);
        }
    }
}
