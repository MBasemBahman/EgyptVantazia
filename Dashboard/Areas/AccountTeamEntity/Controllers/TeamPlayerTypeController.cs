using Dashboard.Areas.AccountTeamEntity.Models;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.AccountTeamEntity.Controllers
{
    [Area("AccountTeamEntity")]
    [Authorize(DashboardViewEnum.TeamPlayerType, AccessLevelEnum.View)]
    public class TeamPlayerTypeController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public TeamPlayerTypeController(ILoggerManager logger, IMapper mapper,
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
            TeamPlayerTypeFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] TeamPlayerTypeFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            RequestParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<TeamPlayerTypeModel> data = await _unitOfWork.AccountTeam.GetTeamPlayerTypePaged(parameters, otherLang);

            List<TeamPlayerTypeDto> resultDto = _mapper.Map<List<TeamPlayerTypeDto>>(data);

            DataTable<TeamPlayerTypeDto> dataTableManager = new();

            DataTableResult<TeamPlayerTypeDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.AccountTeam.GetTeamPlayerTypeCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            TeamPlayerTypeDto data = _mapper.Map<TeamPlayerTypeDto>(_unitOfWork.AccountTeam
                                                           .GetTeamPlayerTypebyId(id, otherLang));

            return View(data);
        }


        [Authorize(DashboardViewEnum.TeamPlayerType, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            TeamPlayerTypeCreateOrEditModel model = new()
            {
                TeamPlayerTypeLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<TeamPlayerTypeCreateOrEditModel>(
                                                await _unitOfWork.AccountTeam.FindTeamPlayerTypebyId(id, trackChanges: false));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.ScoreType, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, TeamPlayerTypeCreateOrEditModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                TeamPlayerType dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<TeamPlayerType>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.AccountTeam.CreateTeamPlayerType(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.AccountTeam.FindTeamPlayerTypebyId(id, trackChanges: true);

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

            return View(model);
        }

        [Authorize(DashboardViewEnum.TeamPlayerType, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            TeamPlayerType data = await _unitOfWork.AccountTeam.FindTeamPlayerTypebyId(id, trackChanges: false);

            return View(data != null && !_unitOfWork.AccountTeam.GetAccountTeamPlayerGameWeaks(new AccountTeamPlayerGameWeakParameters
            {
                Fk_TeamPlayerType = id
            }, otherLang: false).Any());
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.TeamPlayerType, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.AccountTeam.DeleteTeamPlayerType(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }


    }
}
