using Dashboard.Areas.MatchStatisticEntity.Models;
using Dashboard.Areas.PlayerScoreEntity.Models;
using Dashboard.Areas.PlayerStateEntity.Models;
using Dashboard.Areas.SeasonEntity.Models;
using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.MatchStatisticModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.SeasonModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.MatchStatisticEntity.Controllers
{
    [Area("MatchStatisticEntity")]
    [Authorize(DashboardViewEnum.MatchStatisticScore, AccessLevelEnum.View)]
    public class MatchStatisticScoreController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public MatchStatisticScoreController(ILoggerManager logger, IMapper mapper,
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

        public IActionResult Index(int Fk_TeamGameWeak, bool ProfileLayOut = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            MatchStatisticScoreFilter filter = new()
            {
                Fk_TeamGameWeak = Fk_TeamGameWeak,
            };

            ViewData["ProfileLayOut"] = ProfileLayOut;
            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            SetViewData(isProfile: false, id: 0, Fk_StatisticCategory: 0, otherLang);
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] MatchStatisticScoreFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            MatchStatisticScoreParameters parameters = new();

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<MatchStatisticScoreModel> data = await _unitOfWork.MatchStatistic.GetMatchStatisticScoresPaged(parameters, otherLang);

            List<MatchStatisticScoreDto> resultDto = _mapper.Map<List<MatchStatisticScoreDto>>(data);

            DataTable<MatchStatisticScoreDto> dataTableManager = new();

            DataTableResult<MatchStatisticScoreDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.MatchStatistic.GetMatchStatisticScoreCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }


        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            MatchStatisticScoreDto data = _mapper.Map<MatchStatisticScoreDto>
            (_unitOfWork.MatchStatistic.GetMatchStatisticScorebyId(id,otherLang));

            return View(data);
        }


        [Authorize(DashboardViewEnum.MatchStatisticScore, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0, int Fk_TeamGameWeak = 0, bool IsProfile=false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            MatchStatisticScoreCreateOrEditModel model = new();

            if (id > 0)
            {
                model = _mapper.Map<MatchStatisticScoreCreateOrEditModel>(
                                                await _unitOfWork.MatchStatistic.FindMatchStatisticScorebyId(id, trackChanges: false));


                model.Fk_StatisticCategory = _unitOfWork.MatchStatistic.GetStatisticScorebyId(model.Fk_StatisticScore, otherLang: false).Fk_StatisticCategory;
            }
            else
            {
                model.Fk_TeamGameWeak = Fk_TeamGameWeak;
            }

            if (Fk_TeamGameWeak > 0)
            {
                IsProfile = true;
            }
            SetViewData(IsProfile, id, model.Fk_StatisticCategory, otherLang);

       

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.MatchStatisticScore, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, MatchStatisticScoreCreateOrEditModel model, bool IsProfile = false )
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (!ModelState.IsValid)
            {
                SetViewData(IsProfile, id, model.Fk_StatisticCategory, otherLang);

                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                MatchStatisticScore dataDB = new();


                if (id == 0)
                {
                    dataDB = _mapper.Map<MatchStatisticScore>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.MatchStatistic.CreateMatchStatisticScore(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.MatchStatistic.FindMatchStatisticScorebyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;
                }

                await _unitOfWork.Save();

                return IsProfile
                    ? Redirect($"/MatchStatisticEntity/MatchStatisticScore/Index/?Fk_TeamGameWeak={model.Fk_TeamGameWeak}")
                    : RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(IsProfile, id, model.Fk_StatisticCategory, otherLang);

            return View(model);
        }

        [Authorize(DashboardViewEnum.MatchStatisticScore, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            MatchStatisticScore data = await _unitOfWork.MatchStatistic.FindMatchStatisticScorebyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.MatchStatisticScore, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.MatchStatistic.DeleteMatchStatisticScore(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(bool isProfile,int id, int Fk_StatisticCategory, bool otherLang)
        {
            ViewData["GameWeak"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters(), otherLang);
            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);
            ViewData["Team"] = _unitOfWork.Team.GetTeamLookUp(new TeamParameters(), otherLang);
            ViewData["id"] = id;
            ViewData["IsProfile"] = isProfile;
            ViewData["StatisticScore"] = _unitOfWork.MatchStatistic.GetStatisticScoresLookUp(new StatisticScoreParameters()
            {
                Fk_StatisticCategory = Fk_StatisticCategory
            }, otherLang);

            ViewData["StatisticCategory"] = _unitOfWork.MatchStatistic.GetStatisticCategorysLookUp(new StatisticCategoryParameters(), otherLang);
        }
    }
}
