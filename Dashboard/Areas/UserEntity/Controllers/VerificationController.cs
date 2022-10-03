using Dashboard.Areas.UserEntity.Models;
using Entities.CoreServicesModels.UserModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.UserEntity.Controllers
{
    [Area("UserEntity")]
    [Authorize(DashboardViewEnum.Verification, AccessLevelEnum.View)]
    public class VerificationController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public VerificationController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int id, int Fk_User, bool ProfileLayOut = false)
        {
            VerificationFilter filter = new()
            {
                Id = id,
                Fk_User = Fk_User
            };
            ViewData["ProfileLayOut"] = ProfileLayOut;
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] VerificationFilter dtParameters)
        {
            VerificationParameters parameters = new()
            {
                SearchColumns = "Id,Code,CreatedByIp"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<VerificationModel> data = await _unitOfWork.User.GetVerificationsPaged(parameters, trackChanges: false);

            List<Models.VerificationDto> resultDto = _mapper.Map<List<Models.VerificationDto>>(data);

            DataTable<Models.VerificationDto> dataTableManager = new();

            DataTableResult<Models.VerificationDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.User.GetVerificationsCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }
    }
}
