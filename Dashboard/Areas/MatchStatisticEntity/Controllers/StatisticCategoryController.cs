using Dashboard.Areas.MatchStatisticEntity.Models;
using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.DBModels.MatchStatisticModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.MatchStatisticEntity.Controllers
{
    [Area("MatchStatisticEntity")]
    [Authorize(DashboardViewEnum.StatisticCategory, AccessLevelEnum.View)]
    public class StatisticCategoryController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public StatisticCategoryController(ILoggerManager logger, IMapper mapper,
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
            StatisticCategoryFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] StatisticCategoryFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            StatisticCategoryParameters parameters = new()
            {
                SearchColumns = "Id,Name",
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<StatisticCategoryModel> data = await _unitOfWork.MatchStatistic.GetStatisticCategorysPaged(parameters, otherLang);

            List<StatisticCategoryDto> resultDto = _mapper.Map<List<StatisticCategoryDto>>(data);

            DataTable<StatisticCategoryDto> dataTableManager = new();

            DataTableResult<StatisticCategoryDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.MatchStatistic.GetStatisticCategoryCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            StatisticCategoryDto data = _mapper.Map<StatisticCategoryDto>(_unitOfWork.MatchStatistic
                                                           .GetStatisticCategorybyId(id, otherLang));

            return View(data);
        }


        [Authorize(DashboardViewEnum.StatisticCategory, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            StatisticCategoryCreateOrEditModel model = new()
            {
                StatisticCategoryLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<StatisticCategoryCreateOrEditModel>(
                                                await _unitOfWork.MatchStatistic.FindStatisticCategorybyId(id, trackChanges: false));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.StatisticCategory, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, StatisticCategoryCreateOrEditModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                StatisticCategory dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<StatisticCategory>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.MatchStatistic.CreateStatisticCategory(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.MatchStatistic.FindStatisticCategorybyId(id, trackChanges: true);

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

            return View(model);
        }

        [Authorize(DashboardViewEnum.StatisticCategory, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            StatisticCategory data = await _unitOfWork.MatchStatistic.FindStatisticCategorybyId(id, trackChanges: false);

            return View(data != null && !_unitOfWork.MatchStatistic.GetStatisticScoresLookUp(new StatisticScoreParameters
            {
                Fk_StatisticCategory = id
            }, otherLang: false).Any());
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.StatisticCategory, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.MatchStatistic.DeleteStatisticCategory(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }


    }
}
