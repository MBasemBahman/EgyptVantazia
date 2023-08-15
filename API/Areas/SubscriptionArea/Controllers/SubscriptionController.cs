using API.Controllers;
using Entities.CoreServicesModels.SubscriptionModels;
using static Contracts.EnumData.DBModelsEnum;

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
            [FromQuery] _365CompetitionsEnum _365CompetitionsEnum,
            [FromQuery] SubscriptionParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            _365CompetitionsEnum = (_365CompetitionsEnum)auth.Season._365_CompetitionsId.ParseToInt();

            parameters.Fk_Account = auth.Fk_Account;
            parameters.Fk_Season = _unitOfWork.Season.GetCurrentSeasonId(_365CompetitionsEnum);
            parameters.OrderBy = "order";

            PagedList<SubscriptionModel> data = await _unitOfWork.Subscription.GetSubscriptionPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }
    }
}
