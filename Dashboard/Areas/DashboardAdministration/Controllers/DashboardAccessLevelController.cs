using Dashboard.Areas.DashboardAdministration.Models;
using Entities.DBModels.DashboardAdministrationModels;
using Entities.RequestFeatures;
namespace Dashboard.Areas.DashboardAdministration.Controllers
{
    [Area("DashboardAdministration")]
    [Authorize(DashboardViewEnum.DashboardAccessLevel, AccessLevelEnum.View)]
    public class DashboardAccessLevelController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public DashboardAccessLevelController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int id)
        {
            DashboardAccessLevelFilter filter = new()
            {
                Id = id
            };

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] DashboardAccessLevelFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            DashboardAccessLevelParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<DashboardAccessLevelModel> data = await _unitOfWork.DashboardAdministration.GetAccessLevelsPaged(parameters, otherLang);

            List<DashboardAccessLevelDto> resultDto = _mapper.Map<List<DashboardAccessLevelDto>>(data);

            DataTable<DashboardAccessLevelDto> dataTableManager = new();

            DataTableResult<DashboardAccessLevelDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.DashboardAdministration.GetAccessLevelsCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }


        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            DashboardAccessLevelDto data = _mapper.Map<DashboardAccessLevelDto>(_unitOfWork.DashboardAdministration
                                                            .GetAccessLevelbyId(id, otherLang));

            return View(data);
        }

        [Authorize(DashboardViewEnum.DashboardAccessLevel, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            DashboardAccessLevelCreateOrEditModel model = new()
            {
                DashboardAccessLevelLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<DashboardAccessLevelCreateOrEditModel>(
                                                await _unitOfWork.DashboardAdministration.FindAccessLevelById(id, trackChanges: false));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.DashboardAccessLevel, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, DashboardAccessLevelCreateOrEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                DashboardAccessLevel dataDb = new();
                if (id == 0)
                {

                    dataDb = _mapper.Map<DashboardAccessLevel>(model);
                    _unitOfWork.DashboardAdministration.CreateAccessLevel(dataDb);
                }
                else
                {
                    dataDb = await _unitOfWork.DashboardAdministration.FindAccessLevelById(id, trackChanges: true);
                    _ = _mapper.Map(model, dataDb);
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

        [Authorize(DashboardViewEnum.DashboardAccessLevel, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            DashboardAccessLevel data = await _unitOfWork.DashboardAdministration.FindAccessLevelById(id, trackChanges: false);

            return View(data != null && !_unitOfWork.DashboardAdministration.GetPremissions(new AdministrationRolePremissionParameters { Fk_DashboardAccessLevel = id }, otherLang: false).Any());
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.DashboardAccessLevel, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.DashboardAdministration.DeleteAccessLevel(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
