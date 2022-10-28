using Dashboard.Areas.SubscriptionEntity.Models;
using Entities.CoreServicesModels.SubscriptionModels;
using Entities.DBModels.SubscriptionModels;
using Entities.RequestFeatures;
using static Entities.EnumData.LogicEnumData;

namespace Dashboard.Areas.SubscriptionEntity.Controllers
{
    [Area("SubscriptionEntity")]
    [Authorize(DashboardViewEnum.Subscription, AccessLevelEnum.View)]
    public class SubscriptionController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public SubscriptionController(ILoggerManager logger, IMapper mapper,
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

        public IActionResult Index()
        {
            _ = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SubscriptionFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] SubscriptionFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SubscriptionParameters parameters = new()
            {

            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<SubscriptionModel> data = await _unitOfWork.Subscription.GetSubscriptionPaged(parameters, otherLang);

            List<SubscriptionDto> resultDto = _mapper.Map<List<SubscriptionDto>>(data);

            DataTable<SubscriptionDto> dataTableManager = new();

            DataTableResult<SubscriptionDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Subscription.GetSubscriptionCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SubscriptionDto data = _mapper.Map<SubscriptionDto>(_unitOfWork.Subscription
                                                           .GetSubscriptions(new SubscriptionParameters
                                                           {
                                                               Id = id
                                                           }, otherLang).FirstOrDefault());

            return View(data);
        }

        [Authorize(DashboardViewEnum.Subscription, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SubscriptionCreateOrEditModel model = new()
            {
                SubscriptionLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<SubscriptionCreateOrEditModel>(
                                                await _unitOfWork.Subscription.FindSubscriptionById(id, trackChanges: false));
            }

            if (model.ImageUrl.IsNullOrEmpty())
            {
                model.ImageUrl = "calendar-date.png";
                model.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
            }

            SetViewData(otherLang);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.Subscription, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, SubscriptionCreateOrEditModel model)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (!ModelState.IsValid)
            {
                SetViewData(otherLang);

                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                Subscription dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<Subscription>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.Subscription.CreateSubscription(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.Subscription.FindSubscriptionById(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;

                }

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];

                if (imageFile != null)
                {
                    dataDB.ImageUrl = await _unitOfWork.Subscription.UploadSubscriptionImage(_environment.WebRootPath, imageFile);
                    dataDB.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
                }

                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(otherLang);

            return View(model);
        }

        [Authorize(DashboardViewEnum.Subscription, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            Subscription data = await _unitOfWork.Subscription.FindSubscriptionById(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.Subscription, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Subscription.DeleteSubscription(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(bool otherLang)
        {
            ViewData["AppView"] = Enum.GetValues(typeof(AppViewEnum))
                .Cast<AppViewEnum>()
                .ToDictionary(a => ((int)a).ToString(), a => a.ToString());
        }


    }
}
