using Dashboard.Areas.SeasonEntity.Models;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.StandingsModels;
using Entities.DBModels.SeasonModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.SeasonEntity.Controllers
{
    [Area("SeasonEntity")]
    [Authorize(DashboardViewEnum.Season, AccessLevelEnum.View)]
    public class SeasonController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public SeasonController(ILoggerManager logger, IMapper mapper,
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
            _ = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SeasonFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] SeasonFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SeasonParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<SeasonModel> data = await _unitOfWork.Season.GetSeasonPaged(parameters, otherLang);

            List<SeasonDto> resultDto = _mapper.Map<List<SeasonDto>>(data);

            DataTable<SeasonDto> dataTableManager = new();

            DataTableResult<SeasonDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Season.GetSeasonCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SeasonDto data = _mapper.Map<SeasonDto>(_unitOfWork.Season
                                                           .GetSeasonbyId(id, otherLang));

            data.GameWeaks = _mapper.Map<List<GameWeakDto>>(
                _unitOfWork.Season.GetGameWeaks(new GameWeakParameters { Fk_Season = id }, otherLang)
                );

            return View(data);
        }

        public IActionResult Profile(int id, int returnItem = (int)SeasonProfileItems.Details)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SeasonDto data = _mapper.Map<SeasonDto>(_unitOfWork.Season
                                                           .GetSeasonbyId(id, otherLang));

            ViewData["returnItem"] = returnItem;
            ViewData["otherLang"] = otherLang;


            return View(data);
        }

        [Authorize(DashboardViewEnum.Season, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0, int returnPage = (int)SeasonReturnPage.Index)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SeasonCreateOrEditModel model = new()
            {
                SeasonLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<SeasonCreateOrEditModel>(
                                                await _unitOfWork.Season.FindSeasonbyId(id, trackChanges: false));

                model.GameWeaks = _mapper.Map<List<GameWeakCreateOrEditModel>>(
                    _unitOfWork.Season.FindGameWeaks(new GameWeakParameters { Fk_Season = id }, trackChanges: false)
                    );
            }

            if (model.ImageUrl.IsNullOrEmpty())
            {
                model.ImageUrl = "calendar-date.png";
                model.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
            }

            if (model.GameWeaks != null && model.GameWeaks.Any())
            {
                model.GameWeaks.ForEach(a =>
                {
                    if (a.Deadline != null)
                    {
                        a.Deadline = a.Deadline.Value.AddHours(2);
                    }
                });
            }

            SetViewData(returnPage, id, otherLang);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.Season, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, SeasonCreateOrEditModel model, int returnPage = (int)SeasonReturnPage.Index)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (!ModelState.IsValid)
            {
                SetViewData(returnPage, id, otherLang);

                return View(model);
            }
            try
            {
                if (model.GameWeaks != null && model.GameWeaks.Any())
                {
                    model.GameWeaks.ForEach(a =>
                    {
                        if (a.Deadline != null)
                        {
                            a.Deadline = a.Deadline.Value.AddHours(-3);
                        }
                    });
                }

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                Season dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<Season>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.Season.CreateSeason(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.Season.FindSeasonbyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;
                }

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];

                if (imageFile != null)
                {
                    dataDB.ImageUrl = await _unitOfWork.Season.UploudSeasonImage(_environment.WebRootPath, imageFile);
                    dataDB.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
                }

                await _unitOfWork.Save();

                dataDB = await _unitOfWork.Season.UpdateSeasonGameWeaks(dataDB, model.GameWeaks);

                await _unitOfWork.Save();


                return returnPage == (int)SeasonReturnPage.SeasonProfile
                    ? RedirectToAction(nameof(Profile), new { id })
                    : (IActionResult)RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(returnPage, id, otherLang);

            return View(model);
        }

        [Authorize(DashboardViewEnum.Season, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            Season data = await _unitOfWork.Season.FindSeasonbyId(id, trackChanges: false);

            return View(data != null && !_unitOfWork.AccountTeam.GetAccountTeams(new AccountTeamParameters
            {
                Fk_Season = id
            }, otherLang: false).Any() &&
            !_unitOfWork.Standings.GetStandings(new StandingsParameters
            {
                Fk_Season = id
            }, otherLang: false).Any());
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.Season, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Season.DeleteSeason(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(int returnPage, int id, bool otherLang)
        {
            ViewData["returnPage"] = returnPage;
            ViewData["id"] = id;


        }


    }
}
