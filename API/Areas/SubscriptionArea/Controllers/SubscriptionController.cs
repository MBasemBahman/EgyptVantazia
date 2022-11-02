using API.Controllers;
using Entities.CoreServicesModels.SubscriptionModels;

namespace API.Areas.SubscriptionArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Subscription")]
    [ApiExplorerSettings(GroupName = "Subscription")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class SubscriptionController : ExtendControllerBase
    {
        public SubscriptionController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetSubscriptions))]
        public async Task<IEnumerable<SubscriptionModel>> GetSubscriptions(
        [FromQuery] SubscriptionParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<SubscriptionModel> data = await _unitOfWork.Subscription.GetSubscriptionPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }
    }
}
