using API.Controllers;
using Entities.CoreServicesModels.NotificationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Areas.NotificationArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Notification")]
    [ApiExplorerSettings(GroupName = "Notification")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class NotificationController : ExtendControllerBase
    {
        public NotificationController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetNotifications))]
        public async Task<IEnumerable<NotificationModel>> GetNotifications(
        [FromQuery] NotificationParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            parameters.OrderBy = "ShowAt desc";
            parameters.IsActive = true;

            PagedList<NotificationModel> data = await _unitOfWork.Notification.GetNotificationPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpGet]
        [Route(nameof(GetNotificationById))]
        public NotificationModel GetNotificationById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            NotificationModel data = _unitOfWork.Notification.GetNotificationbyId(id, otherLang);

            return data;
        }
    }
}
