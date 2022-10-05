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
        private readonly _365Utils _365Utils;
        public AppAboutController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        _365Utils _365Utils,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        {
            this._365Utils = _365Utils;
        }

        [HttpGet]
        [Route(nameof(GetAppAbout))]
        [AllowAll]
        public async Task<AppAboutModel> GetAppAbout()
        {
            await _365Utils.GetGame();
            await _365Utils.GetGames();
            await _365Utils.GetStandings();

            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AppAboutModel data = await _unitOfWork.AppInfo.GetAppAbouts(new RequestParameters(), otherLang).FirstOrDefaultAsync();

            return data;
        }
    }
}
