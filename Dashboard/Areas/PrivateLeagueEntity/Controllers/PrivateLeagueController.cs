using Dashboard.Areas.PrivateLeagueEntity.Models;
using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.PrivateLeagueModels;
using Entities.RequestFeatures;
using static Entities.EnumData.LogicEnumData;

namespace Dashboard.Areas.PrivateLeagueEntity.Controllers
{
    [Area("PrivateLeagueEntity")]
    [Authorize(DashboardViewEnum.PrivateLeague, AccessLevelEnum.View)]
    public class PrivateLeagueController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public PrivateLeagueController(ILoggerManager logger, IMapper mapper,
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

            PrivateLeagueFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            
            ViewData["auth"] = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
            
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] PrivateLeagueFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            PrivateLeagueParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<PrivateLeagueModel> data = await _unitOfWork.PrivateLeague.GetPrivateLeaguePaged(parameters, otherLang);

            List<PrivateLeagueDto> resultDto = _mapper.Map<List<PrivateLeagueDto>>(data);

            DataTable<PrivateLeagueDto> dataTableManager = new();

            DataTableResult<PrivateLeagueDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.PrivateLeague.GetPrivateLeagueCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PrivateLeagueDto data = _mapper.Map<PrivateLeagueDto>(_unitOfWork.PrivateLeague
                                                           .GetPrivateLeaguebyId(id, otherLang));

            return View(data);
        }


        [Authorize(DashboardViewEnum.PrivateLeague, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            PrivateLeagueCreateOrEditModel model = new();

            if (id > 0)
            {
                model = _mapper.Map<PrivateLeagueCreateOrEditModel>(
                                                await _unitOfWork.PrivateLeague.FindPrivateLeaguebyId(id, trackChanges: false));
            }

            model.Fk_Season = model.Fk_GameWeak != null
           ? _unitOfWork.Season.GetGameWeakbyId((int)model.Fk_GameWeak, otherLang: false).Fk_Season
           : _unitOfWork.Season.GetSeasons(new SeasonParameters(), otherLang: false).Any()
               ? _unitOfWork.Season.GetSeasons(new SeasonParameters(), otherLang: false).FirstOrDefault().Id
               : 0;
            SetViewData(model.Fk_Season);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.PrivateLeague, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, PrivateLeagueCreateOrEditModel model)
        {

            if (!ModelState.IsValid)
            {
                SetViewData(model.Fk_Season);

                return View(model);
            }
            try
            {
                model.Fk_GameWeak = model.Fk_GameWeak > 0 ? model.Fk_GameWeak : null;


                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                PrivateLeague dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<PrivateLeague>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.PrivateLeague.CreatePrivateLeague(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.PrivateLeague.FindPrivateLeaguebyId(id, trackChanges: true);

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

            SetViewData(model.Fk_Season);

            return View(model);
        }

        [Authorize(DashboardViewEnum.PrivateLeague, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            PrivateLeague data = await _unitOfWork.PrivateLeague.FindPrivateLeaguebyId(id, trackChanges: false);

            return View(data != null && !_unitOfWork.PrivateLeague.GetPrivateLeagueMembers(new PrivateLeagueMemberParameters
            {
                Fk_PrivateLeague = id
            }).Any());
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.PrivateLeague, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.PrivateLeague.DeletePrivateLeague(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData( int fk_Season)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

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
