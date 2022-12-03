using Dashboard.Areas.SeasonEntity.Models;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.SeasonModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.SeasonEntity.Controllers
{
    [Area("SeasonEntity")]
    [Authorize(DashboardViewEnum.TeamGameWeak, AccessLevelEnum.View)]
    public class TeamGameWeakController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public TeamGameWeakController(ILoggerManager logger, IMapper mapper,
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

        public IActionResult Index(List<int> Fk_Teams, int Fk_Team, int Fk_Away, int Fk_Home,
            int Fk_Season, bool ProfileLayOut = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            TeamGameWeakFilter filter = new()
            {
                Fk_Teams = Fk_Teams,
                Fk_Team = Fk_Team,
                Fk_Away = Fk_Away,
                Fk_Home = Fk_Home,
                Fk_Season = Fk_Season
            };

            ViewData["ProfileLayOut"] = ProfileLayOut;
            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            SetViewData(returnPage: 0, id: 0, fk_Season: 0, otherLang);
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] TeamGameWeakFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            TeamGameWeakParameters parameters = new()
            {
                SearchColumns = "Id"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<TeamGameWeakModel> data = await _unitOfWork.Season.GetTeamGameWeakPaged(parameters, otherLang);

            List<TeamGameWeakDto> resultDto = _mapper.Map<List<TeamGameWeakDto>>(data);

            DataTable<TeamGameWeakDto> dataTableManager = new();

            DataTableResult<TeamGameWeakDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Season.GetTeamGameWeakCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            TeamGameWeakDto data = _mapper.Map<TeamGameWeakDto>(_unitOfWork.Season
                                                           .GetTeamGameWeakbyId(id, otherLang));

            return View(data);
        }


        [Authorize(DashboardViewEnum.TeamGameWeak, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0, int Fk_Team = 0, int returnPage = (int)TeamGameWeakReturnPage.Index)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            TeamGameWeakCreateOrEditModel model = new();

            if (id > 0)
            {
                model = _mapper.Map<TeamGameWeakCreateOrEditModel>(
                                                await _unitOfWork.Season.FindTeamGameWeakbyId(id, trackChanges: false));
            }
            else
            {
                model.StartTime = DateTime.UtcNow;
            }
            model.Fk_Season = model.Fk_GameWeak > 0
                ? _unitOfWork.Season.GetGameWeakbyId(model.Fk_GameWeak, otherLang: false).Fk_Season
                : _unitOfWork.Season.GetSeasons(new SeasonParameters(), otherLang: false).Any()
                    ? _unitOfWork.Season.GetSeasons(new SeasonParameters(), otherLang: false).FirstOrDefault().Id
                    : 0;

            SetViewData(returnPage, id, model.Fk_Season, otherLang);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.TeamGameWeak, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, TeamGameWeakCreateOrEditModel model, int returnPage = (int)TeamGameWeakReturnPage.Index)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (!ModelState.IsValid)
            {
                SetViewData(returnPage, id, model.Fk_Season, otherLang);

                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                TeamGameWeak dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<TeamGameWeak>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.Season.CreateTeamGameWeak(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.Season.FindTeamGameWeakbyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;
                }



                await _unitOfWork.Save();

                return returnPage == (int)TeamGameWeakReturnPage.SeasonProfile
                    ? Redirect($"/SeasonEntity/Season/Profile/{model.Fk_Season}")
                    : returnPage == (int)TeamGameWeakReturnPage.TeamProfile
                    ? Redirect($"/TeamEntity/Team/Profile/{model.Fk_Home}")
                    : RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(returnPage, id, model.Fk_Season, otherLang);

            return View(model);
        }

        [Authorize(DashboardViewEnum.TeamGameWeak, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            TeamGameWeak data = await _unitOfWork.Season.FindTeamGameWeakbyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.TeamGameWeak, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Season.DeleteTeamGameWeak(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(int returnPage, int id, int fk_Season, bool otherLang)
        {
            ViewData["returnPage"] = returnPage;
            ViewData["id"] = id;
            ViewData["GameWeak"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters()
            {
                Fk_Season = fk_Season
            }, otherLang);
            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);
            ViewData["Team"] = _unitOfWork.Team.GetTeamLookUp(new TeamParameters(), otherLang);


        }


    }
}
