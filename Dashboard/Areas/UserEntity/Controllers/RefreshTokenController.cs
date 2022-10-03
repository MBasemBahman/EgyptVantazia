using Dashboard.Areas.UserEntity.Models;
using Entities.CoreServicesModels.UserModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.UserEntity.Controllers
{
    [Area("UserEntity")]
    [Authorize(DashboardViewEnum.RefreshToken, AccessLevelEnum.View)]
    public class RefreshTokenController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public RefreshTokenController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int id, int Fk_User, bool ProfileLayOut = false)
        {
            RefreshTokenFilter filter = new()
            {
                Id = id,
                Fk_User = Fk_User
            };
            ViewData["ProfileLayOut"] = ProfileLayOut;
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] RefreshTokenFilter dtParameters)
        {
            RefreshTokenParameters parameters = new()
            {
                SearchColumns = "Id,Token,CreatedByIp,RevokedByIp,ReplacedByToken"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<RefreshTokenModel> data = await _unitOfWork.User.GetRefreshTokensPaged(parameters, trackChanges: false);

            List<RefreshTokenDto> resultDto = _mapper.Map<List<RefreshTokenDto>>(data);

            DataTable<RefreshTokenDto> dataTableManager = new();

            DataTableResult<RefreshTokenDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.User.GetRefreshTokensCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }
    }
}
