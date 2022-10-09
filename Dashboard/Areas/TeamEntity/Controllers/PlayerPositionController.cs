using Dashboard.Areas.TeamEntity.Models;
using Dashboard.Areas.Dashboard.Models;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.AccountModels;
using Entities.RequestFeatures;
using Entities.DBModels.TeamModels;

namespace Dashboard.Areas.TeamEntity.Controllers
{
    [Area("TeamEntity")]
    [Authorize(DashboardViewEnum.PlayerPosition, AccessLevelEnum.View)]
    public class PlayerPositionController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public PlayerPositionController(ILoggerManager logger, IMapper mapper,
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

            PlayerPositionFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] PlayerPositionFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerPositionParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<PlayerPositionModel> data = await _unitOfWork.Team.GetPlayerPositionPaged(parameters, otherLang);

            List<PlayerPositionDto> resultDto = _mapper.Map<List<PlayerPositionDto>>(data);

            DataTable<PlayerPositionDto> dataTableManager = new();

            DataTableResult<PlayerPositionDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Team.GetPlayerPositionCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerPositionDto data = _mapper.Map<PlayerPositionDto>(_unitOfWork.Team
                                                           .GetPlayerPositionbyId(id, otherLang));

            return View(data);
        }

     
        [Authorize(DashboardViewEnum.PlayerPosition, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            PlayerPositionCreateOrEditModel model = new()
            {
                PlayerPositionLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<PlayerPositionCreateOrEditModel>(
                                                await _unitOfWork.Team.FindPlayerPositionbyId(id, trackChanges: false));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.PlayerPosition, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, PlayerPositionCreateOrEditModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                PlayerPosition dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<PlayerPosition>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.Team.CreatePlayerPosition(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.Team.FindPlayerPositionbyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;
                }

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];

                if (imageFile != null)
                {
                    dataDB.ImageUrl = await _unitOfWork.Team.UploudPlayerPositionImage(_environment.WebRootPath, imageFile);
                    dataDB.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
                }

                await _unitOfWork.Save();

                return (IActionResult)RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            return View(model);
        }

        [Authorize(DashboardViewEnum.PlayerPosition, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            PlayerPosition data = await _unitOfWork.Team.FindPlayerPositionbyId(id, trackChanges: false);

            return View(data != null && !_unitOfWork.Team.GetPlayers(new PlayerParameters
            {
                Fk_PlayerPosition = id
            },otherLang:false).Any());
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.PlayerPosition, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Team.DeletePlayerPosition(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }


    }
}
