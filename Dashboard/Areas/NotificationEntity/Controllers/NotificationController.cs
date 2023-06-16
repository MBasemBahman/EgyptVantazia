using Dashboard.Areas.NotificationEntity.Models;
using Entities.CoreServicesModels.NotificationModels;
using Entities.DBModels.NotificationModels;
using Entities.RequestFeatures;
using static Entities.EnumData.LogicEnumData;

namespace Dashboard.Areas.NotificationEntity.Controllers
{
    [Area("NotificationEntity")]
    [Authorize(DashboardViewEnum.Notification, AccessLevelEnum.View)]
    public class NotificationController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;
        private readonly IFirebaseNotificationManager _notificationManager;

        public NotificationController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork,
                 LinkGenerator linkGenerator,
                 IWebHostEnvironment environment, IFirebaseNotificationManager notificationManager)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _linkGenerator = linkGenerator;
            _environment = environment;
            _notificationManager = notificationManager;
        }

        public IActionResult Index()
        {
            _ = (bool)Request.HttpContext.Items[ApiConstants.Language];

            NotificationFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] NotificationFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            NotificationParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<NotificationModel> data = await _unitOfWork.Notification.GetNotificationPaged(parameters, otherLang);

            List<NotificationDto> resultDto = _mapper.Map<List<NotificationDto>>(data);

            DataTable<NotificationDto> dataTableManager = new();

            DataTableResult<NotificationDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Notification.GetNotificationCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            NotificationDto data = _mapper.Map<NotificationDto>(_unitOfWork.Notification
                                                           .GetNotifications(new NotificationParameters
                                                           {
                                                               Id = id
                                                           }, otherLang).FirstOrDefault());

            return View(data);
        }

        [Authorize(DashboardViewEnum.Notification, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            NotificationCreateOrEditModel model = new()
            {
                NotificationLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<NotificationCreateOrEditModel>(
                                                await _unitOfWork.Notification.FindNotificationbyId(id, trackChanges: false));
            }

            if (model.ImageUrl.IsNullOrEmpty())
            {
                model.ImageUrl = "calendar-date.png";
                model.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
            }

            SetViewData(otherLang, id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.Notification, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(
            int id,
            NotificationCreateOrEditModel model,
            bool sendNotification)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (!ModelState.IsValid)
            {
                SetViewData(otherLang, id);

                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                Notification dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<Notification>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.Notification.CreateNotification(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.Notification.FindNotificationbyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;

                }

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];

                if (imageFile != null)
                {
                    dataDB.ImageUrl = await _unitOfWork.Notification.UploadNotificationImage(_environment.WebRootPath, imageFile);
                    dataDB.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
                }

                await _unitOfWork.Save();

                if (sendNotification)
                {
                    _notificationManager.SendToTopic(new FirebaseNotificationModel
                    {
                        MessageHeading = dataDB.Title,
                        MessageContent = dataDB.Description,
                        ImgUrl = dataDB.StorageUrl + dataDB.ImageUrl,
                        OpenType = dataDB.OpenType.ToString(),
                        OpenValue = dataDB.OpenValue.IsEmpty() ? "0" : dataDB.OpenValue,
                        Topic = model.Topic
                    }).Wait();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(otherLang, id);

            return View(model);
        }

        [Authorize(DashboardViewEnum.Notification, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            Notification data = await _unitOfWork.Notification.FindNotificationbyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.Notification, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Notification.DeleteNotification(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(bool otherLang, int id = 0)
        {
            ViewData["id"] = id;
            ViewData["AppView"] = Enum.GetValues(typeof(AppViewEnum))
                .Cast<AppViewEnum>()
                .ToDictionary(a => ((int)a).ToString(), a => a.ToString());

            ViewData["News"] = _unitOfWork.News.GetNews(new Entities.CoreServicesModels.NewsModels.NewsParameters
            {

            }, otherLang).OrderByDescending(a => a.CreatedAt).ToDictionary(a => a.Id.ToString(), a => a.Title);

            ViewData["GameWeak"] = _unitOfWork.Season.GetGameWeakLookUp(new Entities.CoreServicesModels.SeasonModels.GameWeakParameters
            {

            }, otherLang);

            ViewData["Team"] = _unitOfWork.Team.GetTeamLookUp(new Entities.CoreServicesModels.TeamModels.TeamParameters
            {

            }, otherLang);
        }


    }
}
