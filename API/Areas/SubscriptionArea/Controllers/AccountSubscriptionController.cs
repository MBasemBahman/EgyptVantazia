using API.Controllers;
using Entities.CoreServicesModels.AccountModels;

namespace API.Areas.SubscriptionArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Subscription")]
    [ApiExplorerSettings(GroupName = "Subscription")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class AccountSubscriptionController : ExtendControllerBase
    {
        public AccountSubscriptionController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(MySubscriptions))]
        public async Task<IEnumerable<AccountSubscriptionModel>> MySubscriptions(
        [FromQuery] AccountSubscriptionParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            parameters.Fk_Account = auth.Fk_Account;

            PagedList<AccountSubscriptionModel> data = await _unitOfWork.Account.GetAccountSubscriptionsPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }
    }
}
