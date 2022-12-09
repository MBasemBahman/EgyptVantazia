using Dashboard.Areas.PlayerScoreEntity.Models;
using Dashboard.Areas.TeamEntity.Models;
using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.PlayerScoreEntity.Controllers
{
    [Area("PlayerScoreEntity")]
    [Authorize(DashboardViewEnum.PlayerGameWeak, AccessLevelEnum.View)]
    public class PlayerGameWeakController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public PlayerGameWeakController(ILoggerManager logger, IMapper mapper,
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

        public IActionResult Index(int Fk_Player, bool ProfileLayOut = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerGameWeakFilter filter = new()
            {
                Fk_Player = Fk_Player
            };

            ViewData["ProfileLayOut"] = ProfileLayOut;
            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            ViewData["Players"] = _unitOfWork.Team.GetPlayerLookUp(new PlayerParameters(), otherLang);
            ViewData["Team"] = _unitOfWork.Team.GetTeamLookUp(new TeamParameters(), otherLang);
            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);
            ViewData["GameWeak"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters(), otherLang);
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] PlayerGameWeakFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerGameWeakParameters parameters = new()
            {
                SearchColumns = ""
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<PlayerGameWeakModel> data = await _unitOfWork.PlayerScore.GetPlayerGameWeakPaged(parameters, otherLang);

            List<PlayerGameWeakDto> resultDto = _mapper.Map<List<PlayerGameWeakDto>>(data);

            DataTable<PlayerGameWeakDto> dataTableManager = new();

            DataTableResult<PlayerGameWeakDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.PlayerScore.GetPlayerGameWeakCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerGameWeakDto data = _mapper.Map<PlayerGameWeakDto>(_unitOfWork.PlayerScore
                                                           .GetPlayerGameWeakbyId(id, otherLang));


            data.PlayerGameWeakScores = _mapper.Map<List<PlayerGameWeakScoreDto>>(
             _unitOfWork.PlayerScore.GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters
             { Fk_PlayerGameWeak = id }, otherLang)
             );
            return View(data);
        }

        public IActionResult Profile(int id, int returnItem = (int)PlayerGameWeakProfileItems.Details)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerGameWeakDto data = _mapper.Map<PlayerGameWeakDto>(_unitOfWork.PlayerScore
                .GetPlayerGameWeakbyId(id, otherLang));

            ViewData["returnItem"] = returnItem;
            ViewData["otherLang"] = otherLang;


            return View(data);
        }


        [Authorize(DashboardViewEnum.PlayerGameWeak, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0, int Fk_Player = 0)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerGameWeakCreateOrEditModel model = new();

            if (id > 0)
            {
                model = _mapper.Map<PlayerGameWeakCreateOrEditModel>(
                                                await _unitOfWork.PlayerScore.FindPlayerGameWeakbyId(id, trackChanges: false));


                model.PlayerGameWeakScores = _mapper.Map<List<PlayerGameWeakScoreCreateOrEditModel>>(
                _unitOfWork.PlayerScore.GetPlayerGameWeakScores(new PlayerGameWeakScoreParameters
                { Fk_PlayerGameWeak = id }, otherLang)
                );
            }
            if (Fk_Player > 0)
            {
                model.Fk_Player = Fk_Player;
            }
            //model.Fk_Season = model.Fk_GameWeak > 0
            //    ? _unitOfWork.Season.GetGameWeakbyId(model.Fk_GameWeak, otherLang: false).Fk_Season
            //    : _unitOfWork.Season.GetSeasons(new SeasonParameters(), otherLang: false).Any()
            //        ? _unitOfWork.Season.GetSeasons(new SeasonParameters(), otherLang: false).FirstOrDefault().Id
            //        : 0;

            SetViewData(model.Fk_Season, otherLang);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.PlayerGameWeak, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, PlayerGameWeakCreateOrEditModel model)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (!ModelState.IsValid)
            {
                SetViewData(model.Fk_Season, otherLang);

                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                PlayerGameWeak dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<PlayerGameWeak>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.PlayerScore.CreatePlayerGameWeak(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.PlayerScore.FindPlayerGameWeakbyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;
                }



                await _unitOfWork.Save();

                dataDB = await _unitOfWork.PlayerScore.UpdatePlayerGameWeakScores(dataDB, model.PlayerGameWeakScores);

                await _unitOfWork.Save();

                return Redirect($"/TeamEntity/Player/Profile/{model.Fk_Player}?returnItem={(int)PlayerProfileItems.PlayerGameWeak}");
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(model.Fk_Season, otherLang);

            return View(model);
        }

        [Authorize(DashboardViewEnum.PlayerGameWeak, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            PlayerGameWeak data = await _unitOfWork.PlayerScore.FindPlayerGameWeakbyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.PlayerGameWeak, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.PlayerScore.DeletePlayerGameWeak(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(int fk_Season, bool otherLang)
        {

            ViewData["GameWeak"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters()
            {
                Fk_Season = fk_Season
            }, otherLang);
            ViewData["Season"] = _unitOfWork.Season.GetSeasonLookUp(new SeasonParameters(), otherLang);

            ViewData["ScoreType"] = _unitOfWork.PlayerScore.GetScoreTypesLookUp(new ScoreTypeParameters(), otherLang);


        }


    }
}
