using Dashboard.Areas.TeamEntity.Models;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.TeamEntity.Controllers
{
    [Area("TeamEntity")]
    [Authorize(DashboardViewEnum.Player, AccessLevelEnum.View)]
    public class PlayerController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public PlayerController(ILoggerManager logger, IMapper mapper,
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

        public IActionResult Index(int Fk_Team, int Fk_GameWeak, bool ProfileLayOut = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerFilter filter = new()
            {
                Fk_Team = Fk_Team,
                Fk_GameWeak = Fk_GameWeak
            };

            ViewData["ProfileLayOut"] = ProfileLayOut;
            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            SetViewData(returnPage: 0, id: 0, otherLang);
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] PlayerFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<PlayerModel> data = await _unitOfWork.Team.GetPlayerPaged(parameters, otherLang);

            List<PlayerDto> resultDto = _mapper.Map<List<PlayerDto>>(data);

            DataTable<PlayerDto> dataTableManager = new();

            DataTableResult<PlayerDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Team.GetPlayerCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerDto data = _mapper.Map<PlayerDto>(_unitOfWork.Team
                                                           .GetPlayerbyId(id, otherLang));

            data.PlayerPrices = _mapper.Map<List<PlayerPriceDto>>(
                _unitOfWork.Team.GetPlayerPrices(new PlayerPriceParameters { Fk_Player = id }, otherLang)
                );

            return View(data);
        }

        public IActionResult Profile(int id, int returnItem = (int)PlayerProfileItems.Details)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerDto data = _mapper.Map<PlayerDto>(_unitOfWork.Team
                                                           .GetPlayerbyId(id, otherLang));

            ViewData["returnItem"] = returnItem;
            ViewData["otherLang"] = otherLang;


            return View(data);
        }

        [Authorize(DashboardViewEnum.Player, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0, int Fk_Team = 0, int returnPage = (int)PlayerReturnPage.Index)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerCreateOrEditModel model = new()
            {
                PlayerLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<PlayerCreateOrEditModel>(
                                                await _unitOfWork.Team.FindPlayerbyId(id, trackChanges: false));

                model.PlayerPrices = _mapper.Map<List<PlayerPriceCreateOrEditModel>>(
                    _unitOfWork.Team.GetPlayerPrices(new PlayerPriceParameters { Fk_Player = id }, otherLang: false)
                    );
            }
            if (Fk_Team > 0)
            {
                model.Fk_Team = Fk_Team;
                returnPage = (int)PlayerReturnPage.TeamProfile;
            }

            if (model.ImageUrl.IsNullOrEmpty())
            {
                model.ImageUrl = "football-player.png";
                model.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
            }

            SetViewData(returnPage, id, otherLang);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.Player, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, PlayerCreateOrEditModel model, int returnPage = (int)PlayerReturnPage.Index)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (!ModelState.IsValid)
            {
                SetViewData(returnPage, id, otherLang);

                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                Player dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<Player>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.Team.CreatePlayer(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.Team.FindPlayerbyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;
                }

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];

                if (imageFile != null)
                {
                    dataDB.ImageUrl = await _unitOfWork.Team.UploudPlayerImage(_environment.WebRootPath, imageFile);
                    dataDB.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
                }

                await _unitOfWork.Save();

                dataDB = await _unitOfWork.Team.UpdatePlayerPrices(dataDB, model.PlayerPrices);

                await _unitOfWork.Save();


                if (returnPage == (int)PlayerReturnPage.PlayerProfile)
                {
                    return RedirectToAction(nameof(Profile), new { id });
                }
                else if (returnPage == (int)PlayerReturnPage.TeamProfile)
                {
                    return RedirectToAction(nameof(Profile), "Team", new { id = model.Fk_Team, returnItem = (int)TeamProfileItems.Player });

                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(returnPage, id, otherLang);

            return View(model);
        }

        [Authorize(DashboardViewEnum.Player, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            Player data = await _unitOfWork.Team.FindPlayerbyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.Player, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Team.DeletePlayer(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        
        [Authorize(DashboardViewEnum.Player, AccessLevelEnum.CreateOrEdit)]
        public IActionResult EditPlayersPrices([FromQuery]string fk_Players)
        {
            var fk_PlayersIds = fk_Players.Split(",").Select(Int32.Parse).ToList();
            
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            
            ViewData["Players"] = _unitOfWork.Team.GetPlayers(new PlayerParameters
            {
                Fk_Players = fk_PlayersIds
            }, otherLang).ToList();
            
            return View();
        }
        
        [HttpPost]
        [Authorize(DashboardViewEnum.Player, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> EditPlayersPrices(List<PlayerPriceEditModel> model)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            
            if (!ModelState.IsValid)
            {
                ViewData["Players"] = _unitOfWork.Team.GetPlayers(new PlayerParameters
                {
                    Fk_Players = model.Select(a => a.Fk_Player).ToList()
                }, otherLang).ToList();
                
                return View(model);
            }
            
            try
            {
                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                
                var dataDB = _mapper.Map<List<PlayerPrice>>(model);

                _unitOfWork.Team.AddPlayersPrices(dataDB, auth.UserName);

                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }
            
            ViewData["Players"] = _unitOfWork.Team.GetPlayers(new PlayerParameters
            {
                Fk_Players = model.Select(a => a.Fk_Player).ToList()
            }, otherLang).ToList();
            return View();
        }


        // helper methods
        private void SetViewData(int returnPage, int id, bool otherLang)
        {
            ViewData["returnPage"] = returnPage;
            ViewData["id"] = id;
            ViewData["PlayerPosition"] = _unitOfWork.Team.GetPlayerPositionLookUp(new PlayerPositionParameters(), otherLang);
            ViewData["Team"] = _unitOfWork.Team.GetTeamLookUp(new TeamParameters(), otherLang);
        }


    }
}
