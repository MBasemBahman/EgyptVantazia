using API.Controllers;
using Entities.CoreServicesModels.AccountModels;

namespace API.Areas.PaymentArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Payment")]
    [ApiExplorerSettings(GroupName = "Payment")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class PaymentController : ExtendControllerBase
    {
        private readonly PaymobServices _paymobServices;
        public PaymentController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings,
        PaymobServices paymobServices) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        {
            _paymobServices = paymobServices;
        }

        [HttpGet]
        [Route(nameof(MyPayments))]
        public async Task<IEnumerable<PaymentModel>> MyPayments(
       [FromQuery] PaymentParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            parameters.Fk_Account = auth.Fk_Account;

            PagedList<PaymentModel> data = await _unitOfWork.Account.GetPaymentsPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }

    }
}
