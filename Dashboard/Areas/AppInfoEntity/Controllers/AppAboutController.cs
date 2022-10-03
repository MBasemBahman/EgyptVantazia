using CoreServices;
using Dashboard.Areas.AppInfoEntity.Models;
using Entities.CoreServicesModels.AppInfoModels;
using Entities.DBModels.AppInfoModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.AppInfoEntity.Controllers
{
    [Area("AppInfoEntity")]
    [Authorize(DashboardViewEnum.AppAbout, AccessLevelEnum.View)]
    public class AppInfoController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public AppInfoController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Details()
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AppAboutModel model = _unitOfWork.AppInfo.GetAppAbouts(new RequestParameters(),otherLang).FirstOrDefault();

            if (model == null)
            {
                return View(new AppAboutDto());
            }

            AppAboutDto data = _mapper.Map<AppAboutDto>(model);

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            return View(data);
        }

        [Authorize(DashboardViewEnum.AppAbout, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit()
        {
            AppAbout dataDb = await _unitOfWork.AppInfo.FindAppAbout(trackChanges: false);

            if (dataDb == null)
            {
                return View(new AppAboutCreateOrEditModel());
            }

            AppAboutCreateOrEditModel model = _mapper.Map<AppAboutCreateOrEditModel>(dataDb);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.AppAbout, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(AppAboutCreateOrEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

                AppAbout dataDb = await _unitOfWork.AppInfo.FindAppAbout(trackChanges: true);

                if (dataDb == null)
                {
                    dataDb = _mapper.Map<AppAbout>(model);
                    dataDb.CreatedBy = auth.UserName;

                    _unitOfWork.AppInfo.CreateAppAbout(dataDb);
                }
                else
                {
                    _ = _mapper.Map(model, dataDb);

                    dataDb.LastModifiedBy = auth.UserName;
                }

                await _unitOfWork.Save();

                return RedirectToAction(nameof(Details));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            return View(model);
        }
    }
}
