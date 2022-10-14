using Dashboard.Areas.TeamEntity.Models;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.TeamEntity.Controllers
{
    [Area("TeamEntity")]
    [Authorize(DashboardViewEnum.Team, AccessLevelEnum.View)]
    public class TeamController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public TeamController(ILoggerManager logger, IMapper mapper,
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

            TeamFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] TeamFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            TeamParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<TeamModel> data = await _unitOfWork.Team.GetTeamPaged(parameters, otherLang);

            List<TeamDto> resultDto = _mapper.Map<List<TeamDto>>(data);

            DataTable<TeamDto> dataTableManager = new();

            DataTableResult<TeamDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Team.GetTeamCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            TeamDto data = _mapper.Map<TeamDto>(_unitOfWork.Team
                                                           .GetTeambyId(id, otherLang));

            return View(data);
        }

        public IActionResult Profile(int id, int returnItem = (int)TeamProfileItems.Details)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            TeamDto data = _mapper.Map<TeamDto>(_unitOfWork.Team
                                                           .GetTeambyId(id, otherLang));

            ViewData["returnItem"] = returnItem;
            ViewData["otherLang"] = otherLang;


            return View(data);
        }

        [Authorize(DashboardViewEnum.Team, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0, bool IsProfile = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            TeamCreateOrEditModel model = new()
            {
                TeamLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<TeamCreateOrEditModel>(
                                                await _unitOfWork.Team.FindTeambyId(id, trackChanges: false));
            }

            if (model.ImageUrl.IsNullOrEmpty())
            {
                model.ImageUrl = "banner.png";
                model.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
            }

            SetViewData(IsProfile, id, otherLang);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.Team, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, TeamCreateOrEditModel model, bool IsProfile = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (!ModelState.IsValid)
            {
                SetViewData(IsProfile, id, otherLang);

                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                Team dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<Team>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.Team.CreateTeam(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.Team.FindTeambyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;
                }

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];

                if (imageFile != null)
                {
                    dataDB.ImageUrl = await _unitOfWork.Team.UploudTeamImage(_environment.WebRootPath, imageFile);
                    dataDB.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
                }

                await _unitOfWork.Save();

                return IsProfile ? RedirectToAction(nameof(Profile), new { id }) : (IActionResult)RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(IsProfile, id, otherLang);

            return View(model);
        }

        [Authorize(DashboardViewEnum.Team, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            Team data = await _unitOfWork.Team.FindTeambyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.Team, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Team.DeleteTeam(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(bool IsProfile, int id, bool otherLang)
        {
            ViewData["IsProfile"] = IsProfile;
            ViewData["id"] = id;

        }


    }
}
