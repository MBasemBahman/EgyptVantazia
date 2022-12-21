using API.Controllers;
using Entities.CoreServicesModels.AppInfoModels;

namespace API.Areas.AppInfoArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("AppInfo")]
    [ApiExplorerSettings(GroupName = "AppInfo")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class AppAboutController : ExtendControllerBase
    {
        public AppAboutController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        {
        }

        [HttpGet]
        [Route(nameof(GetAppAbout))]
        [AllowAll]
        public async Task<AppAboutModel> GetAppAbout()
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AppAboutModel data = await _unitOfWork.AppInfo.GetAppAbouts(new RequestParameters(), otherLang).FirstOrDefaultAsync();
            data.ShowPayment = false;
            data.ShowInvite = true;

            return data;
        }
    }
}
