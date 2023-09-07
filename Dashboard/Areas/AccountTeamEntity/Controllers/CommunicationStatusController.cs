using Dashboard.Areas.AccountTeamEntity.Models;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.AccountTeamEntity.Controllers
{
    [Area("AccountTeamEntity")]
    [Authorize(DashboardViewEnum.CommunicationStatus, AccessLevelEnum.View)]
    public class CommunicationStatusController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public CommunicationStatusController(ILoggerManager logger, IMapper mapper,
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
            CommunicationStatusFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] CommunicationStatusFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            RequestParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<CommunicationStatusModel> data = await _unitOfWork.AccountTeam.GetCommunicationStatusPaged(parameters, otherLang);

            List<CommunicationStatusDto> resultDto = _mapper.Map<List<CommunicationStatusDto>>(data);

            DataTable<CommunicationStatusDto> dataTableManager = new();

            DataTableResult<CommunicationStatusDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.AccountTeam.GetCommunicationStatusCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            CommunicationStatusDto data = _mapper.Map<CommunicationStatusDto>(_unitOfWork.AccountTeam
                                                           .GetCommunicationStatusbyId(id, otherLang));

            return View(data);
        }


        [Authorize(DashboardViewEnum.CommunicationStatus, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            CommunicationStatusCreateOrEditModel model = new();

            if (id > 0)
            {
                model = _mapper.Map<CommunicationStatusCreateOrEditModel>(
                                                await _unitOfWork.AccountTeam.FindCommunicationStatusbyId(id, trackChanges: false));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.ScoreType, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, CommunicationStatusCreateOrEditModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                CommunicationStatus dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<CommunicationStatus>(model);
                    
                    _unitOfWork.AccountTeam.CreateCommunicationStatus(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.AccountTeam.FindCommunicationStatusbyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);
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

        [Authorize(DashboardViewEnum.CommunicationStatus, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            CommunicationStatus data = await _unitOfWork.AccountTeam.FindCommunicationStatusbyId(id, trackChanges: false);

            return View(data != null && !_unitOfWork.AccountTeam.GetAccountTeams(new AccountTeamParameters
            {
                Fk_CommunicationStatus = id
            }, otherLang: false).Any());
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.CommunicationStatus, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.AccountTeam.DeleteCommunicationStatus(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }


    }
}
