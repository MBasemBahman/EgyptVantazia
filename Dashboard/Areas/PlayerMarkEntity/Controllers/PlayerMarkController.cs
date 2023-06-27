using Dashboard.Areas.PlayerMarkEntity.Models;
using Dashboard.Areas.PlayerScoreEntity.Models;
using Dashboard.Areas.TeamEntity.Models;
using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerMarkModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.PlayerMarkEntity.Controllers
{
    [Area("PlayerMarkEntity")]
    [Authorize(DashboardViewEnum.PlayerMark, AccessLevelEnum.View)]
    public class PlayerMarkController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public PlayerMarkController(ILoggerManager logger, IMapper mapper,
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

            PlayerMarkFilter filter = new()
            {
                Fk_Player = Fk_Player
            };

            ViewData["ProfileLayOut"] = ProfileLayOut;
            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            ViewData["Marks"] = _unitOfWork.PlayerMark.GetMarksLookUp(new MarkParameters(), otherLang);
            ViewData["GameWeaks"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters(), otherLang);
            
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] PlayerMarkFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerMarkParameters parameters = new()
            {
                SearchColumns = ""
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<PlayerMarkModel> data = await _unitOfWork.PlayerMark.GetPlayerMarkPaged(parameters, otherLang);

            List<PlayerMarkDto> resultDto = _mapper.Map<List<PlayerMarkDto>>(data);

            DataTable<PlayerMarkDto> dataTableManager = new();

            DataTableResult<PlayerMarkDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.PlayerMark.GetPlayerMarkCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerMarkDto data = _mapper.Map<PlayerMarkDto>(_unitOfWork.PlayerMark
                                                           .GetPlayerMarkbyId(id, otherLang));
            
            return View(data);
        }

        [Authorize(DashboardViewEnum.PlayerMark, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0, int Fk_Player = 0)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerMarkCreateOrEditModel model = new();

            if (id > 0)
            {
                model = _mapper.Map<PlayerMarkCreateOrEditModel>(await _unitOfWork.PlayerMark.FindPlayerMarkbyId(id, trackChanges: false));

                model.Fk_TeamGameWeaks = _unitOfWork.PlayerMark
                    .GetPlayerMarkTeamGameWeaks(new PlayerMarkTeamGameWeakParameters(), otherLang)
                    .Select(a => a.Fk_TeamGameWeak)
                    .ToList();
                
                model.Fk_PlayerMarkReasonMatches = _unitOfWork.PlayerMark
                    .GetPlayerMarkReasonMatches(new PlayerMarkReasonMatchParameters(), otherLang)
                    .Select(a => a.Fk_TeamGameWeak)
                    .ToList();
            }
            if (Fk_Player > 0)
            {
                model.Fk_Player = Fk_Player;
            }

            SetViewData(Fk_Player, otherLang);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.PlayerMark, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, PlayerMarkCreateOrEditModel model)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (!ModelState.IsValid)
            {
                SetViewData(model.Fk_Player, otherLang);

                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                PlayerMark dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<PlayerMark>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.PlayerMark.CreatePlayerMark(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.PlayerMark.FindPlayerMarkbyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;
                }



                await _unitOfWork.Save();

                await _unitOfWork.PlayerMark.UpdatePlayerMarkReasonMatches(dataDB.Id, model.Fk_PlayerMarkReasonMatches, auth.UserName); 
                await _unitOfWork.PlayerMark.UpdatePlayerMarkTeamGameWeaks(dataDB.Id, model.Fk_TeamGameWeaks, auth.UserName);
                
                await _unitOfWork.Save();

                return Redirect($"/TeamEntity/Player/Profile/{model.Fk_Player}?returnItem={(int)PlayerProfileItems.PlayerMark}");
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(model.Fk_Player, otherLang);

            return View(model);
        }

        [Authorize(DashboardViewEnum.PlayerMark, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            PlayerMark data = await _unitOfWork.PlayerMark.FindPlayerMarkbyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.PlayerMark, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.PlayerMark.DeletePlayerMark(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(int fk_Player, bool otherLang)
        {
            int fk_CurrentSeason = _unitOfWork.Season.GetCurrentSeasonId();
            
            ViewData["GameWeaks"] = _unitOfWork.Season.GetGameWeakLookUp(new GameWeakParameters()
            {
                Fk_Season = fk_CurrentSeason
            }, otherLang);

            ViewData["Marks"] = _unitOfWork.PlayerMark.GetMarksLookUp(new MarkParameters(), otherLang);

            PlayerModel player = _unitOfWork.Team.GetPlayerbyId(fk_Player, otherLang);

            ViewData["TeamGameWeaks"] = _unitOfWork.Season.GetTeamGameWeaks(new TeamGameWeakParameters
            {
                Fk_Team = player.Fk_Team
            }, otherLang).OrderBy(a => a.Fk_GameWeak).ToList();
        }


    }
}
