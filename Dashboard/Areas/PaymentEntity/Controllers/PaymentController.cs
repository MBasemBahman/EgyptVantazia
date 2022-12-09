using Dashboard.Areas.PaymentEntity.Models;
using Entities.CoreServicesModels.AccountModels;
using Entities.RequestFeatures;
namespace Dashboard.Areas.PaymentEntity.Controllers
{
    [Area("PaymentEntity")]
    [Authorize(DashboardViewEnum.Payment, AccessLevelEnum.View)]
    public class PaymentController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public PaymentController(ILoggerManager logger, IMapper mapper,
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

        public IActionResult Index(int Fk_Account, bool ProfileLayOut = false)
        {
            PaymentFilter filter = new()
            {
                Fk_Account = Fk_Account
            };

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            SetViewData(ProfileLayOut);
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] PaymentFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PaymentParameters parameters = new()
            {
                SearchColumns = ""
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<PaymentModel> data = await _unitOfWork.Account.GetPaymentsPaged(parameters, otherLang);

            List<PaymentDto> resultDto = _mapper.Map<List<PaymentDto>>(data);

            DataTable<PaymentDto> dataTableManager = new();

            DataTableResult<PaymentDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Account.GetPaymentsCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        // helper methods
        private void SetViewData(bool ProfileLayOut = false)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ViewData["Account"] = _unitOfWork.Account.GetAccountLookUp(new AccountParameters(), otherLang);
            ViewData["ProfileLayOut"] = ProfileLayOut;
        }

    }
}
