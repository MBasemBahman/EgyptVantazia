using Dashboard.Areas.LogEntity.Models;
using Entities.CoreServicesModels.LogModels;
using Entities.DBModels.LogModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.LogEntity.Controllers
{
    [Area("LogEntity")]
    [Authorize(DashboardViewEnum.DBLogs, AccessLevelEnum.View)]
    public class DBLogsController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public DBLogsController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int id)
        {
            LogFilter filter = new()
            {
                Id = id
            };
            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] LogFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            LogParameters parameters = new()
            {
                SearchColumns = "Id,Level,Logger,Details",
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<LogModel> data = await _unitOfWork.Log.GetLogsPaged(parameters, otherLang);

            List<LogDto> resultDto = _mapper.Map<List<LogDto>>(data);

            DataTable<LogDto> dataTableManager = new();

            DataTableResult<LogDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Log.GetLogsCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }


        public IActionResult Details(int id)
        {
            _ = (bool)Request.HttpContext.Items[ApiConstants.Language];

            LogDto data = _mapper.Map<LogDto>(_unitOfWork.Log
                                                        .GetLogbyId(id, trackChanges: true));

            return View(data);
        }

        [Authorize(DashboardViewEnum.DBLogs, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            Log data = await _unitOfWork.Log.FindLogbyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.DBLogs, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Log.DeleteLog(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

    }
}
