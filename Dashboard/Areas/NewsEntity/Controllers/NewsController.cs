using Dashboard.Areas.NewsEntity.Models;
using Entities.CoreServicesModels.NewsModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.NewsModels;
using Entities.RequestFeatures;
using static Entities.EnumData.LogicEnumData;

namespace Dashboard.Areas.NewsEntity.Controllers
{
    [Area("NewsEntity")]
    [Authorize(DashboardViewEnum.News, AccessLevelEnum.View)]
    public class NewsController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public NewsController(ILoggerManager logger, IMapper mapper,
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
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            NewsFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            SetViewData(IsProfile: false, id: 0, otherLang, fk_Season: 0);
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] NewsFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            NewsParameters parameters = new()
            {
                SearchColumns = "Id,Title"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<NewsModel> data = await _unitOfWork.News.GetNewsPaged(parameters, otherLang);

            List<NewsDto> resultDto = _mapper.Map<List<NewsDto>>(data);

            DataTable<NewsDto> dataTableManager = new();

            DataTableResult<NewsDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.News.GetNewsCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            NewsDto data = _mapper.Map<NewsDto>(_unitOfWork.News
                                                           .GetNewsbyId(id, otherLang));

            return View(data);
        }

        public IActionResult Profile(int id, int returnItem = (int)NewsProfileItems.Details)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            NewsDto data = _mapper.Map<NewsDto>(_unitOfWork.News
                                                           .GetNewsbyId(id, otherLang));

            ViewData["returnItem"] = returnItem;
            ViewData["otherLang"] = otherLang;


            return View(data);
        }

        [Authorize(DashboardViewEnum.News, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0, bool IsProfile = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            NewsCreateOrEditModel model = new()
            {
                NewsLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<NewsCreateOrEditModel>(
                                                await _unitOfWork.News.FindNewsbyId(id, trackChanges: false));


            }

            if (model.ImageUrl.IsNullOrEmpty())
            {
                model.ImageUrl = "news.png";
                model.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
            }

            SetViewData(IsProfile, id, otherLang, model.Fk_Season);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.News, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, NewsCreateOrEditModel model, bool IsProfile = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (!ModelState.IsValid)
            {
                SetViewData(IsProfile, id, otherLang, model.Fk_Season);

                return View(model);
            }
            try
            {
                model.Fk_GameWeak = model.Fk_GameWeak > 0 ? model.Fk_GameWeak : null;

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                News dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<News>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.News.CreateNews(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.News.FindNewsbyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;
                }

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];

                if (imageFile != null)
                {
                    dataDB.ImageUrl = await _unitOfWork.News.UploudNewsImage(_environment.WebRootPath, imageFile);
                    dataDB.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
                }

                await _unitOfWork.Save();

                return IsProfile ? RedirectToAction(nameof(Profile), new { id }) : (IActionResult)RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(IsProfile, id, otherLang, model.Fk_Season);

            return View(model);
        }

        [Authorize(DashboardViewEnum.News, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            News data = await _unitOfWork.News.FindNewsbyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.News, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.News.DeleteNews(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(bool IsProfile, int id, bool otherLang, int fk_Season)
        {
            ViewData["IsProfile"] = IsProfile;
            ViewData["id"] = id;
            ViewData["GameWeak"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters()
            {
                Fk_Season = fk_Season
            }, otherLang);
            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);
            ViewData["NewsType"] = Enum.GetValues(typeof(NewsTypeEnum))
               .Cast<NewsTypeEnum>()
               .ToDictionary(a => ((int)a).ToString(), a => a.ToString());
        }


    }
}
