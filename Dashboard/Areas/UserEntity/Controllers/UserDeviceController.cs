using Dashboard.Areas.UserEntity.Models;
using Entities.CoreServicesModels.UserModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.UserEntity.Controllers
{
    [Area("UserEntity")]
    [Authorize(DashboardViewEnum.UserDevice, AccessLevelEnum.View)]
    public class UserDeviceController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public UserDeviceController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int id, int Fk_User, bool ProfileLayOut = false)
        {
            UserDeviceFilter filter = new()
            {
                Id = id,
                Fk_User = Fk_User
            };
            ViewData["ProfileLayOut"] = ProfileLayOut;
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] UserDeviceFilter dtParameters)
        {
            UserDeviceParameters parameters = new()
            {
                SearchColumns = "Id,NotificationToken,DeviceType,AppVersion,DeviceVersion,DeviceModel"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<UserDeviceModel> data = await _unitOfWork.User.GetDevicesPaged(parameters, trackChanges: false);

            List<UserDeviceDto> resultDto = _mapper.Map<List<UserDeviceDto>>(data);

            DataTable<UserDeviceDto> dataTableManager = new();

            DataTableResult<UserDeviceDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.User.GetDevicesCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

    }
}
