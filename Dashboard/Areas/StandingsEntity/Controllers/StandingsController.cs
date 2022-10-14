using Dashboard.Areas.StandingsEntity.Models;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.StandingsModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.SponsorModels;
using Entities.DBModels.StandingsModels;
using Entities.RequestFeatures;
using static Entities.EnumData.LogicEnumData;

namespace Dashboard.Areas.StandingsEntity.Controllers
{
    [Area("StandingsEntity")]
    [Authorize(DashboardViewEnum.Standings, AccessLevelEnum.View)]
    public class StandingsController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public StandingsController(ILoggerManager logger, IMapper mapper,
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

            StandingsFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            SetViewData(otherLang);
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] StandingsFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            StandingsParameters parameters = new()
            {
                SearchColumns = "Id"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<StandingsModel> data = await _unitOfWork.Standings.GetStandingsPaged(parameters, otherLang);

            List<StandingsDto> resultDto = _mapper.Map<List<StandingsDto>>(data);

            DataTable<StandingsDto> dataTableManager = new();

            DataTableResult<StandingsDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Standings.GetStandingsCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            StandingsDto data = _mapper.Map<StandingsDto>(_unitOfWork.Standings
                                                           .GetStandingsbyId(id, otherLang));




            return View(data);
        }

        [Authorize(DashboardViewEnum.Standings, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            StandingsCreateOrEditModel model = new();

            if (id > 0)
            {
                model = _mapper.Map<StandingsCreateOrEditModel>(
                                                await _unitOfWork.Standings.FindStandingsbyId(id, trackChanges: false));

            }


            SetViewData(otherLang);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.Standings, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, StandingsCreateOrEditModel model)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (!ModelState.IsValid)
            {
                SetViewData(otherLang);

                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                Standings dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<Standings>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.Standings.CreateStandings(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.Standings.FindStandingsbyId(id, trackChanges: true);

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

            SetViewData(otherLang);

            return View(model);
        }

        [Authorize(DashboardViewEnum.Standings, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            Standings data = await _unitOfWork.Standings.FindStandingsbyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.Standings, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Standings.DeleteStandings(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(bool otherLang)
        {
            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);
            ViewData["Team"] = _unitOfWork.Team.GetTeamLookUp(new TeamParameters(), otherLang);
        }


    }
}
