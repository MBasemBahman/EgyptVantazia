using Dashboard.Areas.PromoCodeEntity.Models;
using Dashboard.Areas.SubscriptionEntity.Models;
using Entities.CoreServicesModels.PromoCodeModels;
using Entities.CoreServicesModels.SubscriptionModels;
using Entities.DBModels.PromoCodeModels;
using Entities.RequestFeatures;
using static Entities.EnumData.LogicEnumData;

namespace Dashboard.Areas.PromoCodeEntity.Controllers
{
    [Area("PromoCodeEntity")]
    [Authorize(DashboardViewEnum.PromoCode, AccessLevelEnum.View)]
    public class PromoCodeController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public PromoCodeController(ILoggerManager logger, IMapper mapper,
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

            PromoCodeFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            ViewData["auth"] = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] PromoCodeFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            PromoCodeParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<PromoCodeModel> data = await _unitOfWork.PromoCode.GetPromoCodesPaged(parameters, otherLang);

            List<PromoCodeDto> resultDto = _mapper.Map<List<PromoCodeDto>>(data);

            DataTable<PromoCodeDto> dataTableManager = new();

            DataTableResult<PromoCodeDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.PromoCode.GetPromoCodeCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PromoCodeDto data = _mapper.Map<PromoCodeDto>(_unitOfWork.PromoCode
                                                           .GetPromoCodebyId(id, otherLang));

            data.Subscriptions =_mapper.Map<List<SubscriptionDto>>(_unitOfWork.PromoCode.GetPromoCodeSubscriptions(new PromoCodeSubscriptionParameters
            {
                Fk_PromoCode = id
            }, otherLang).Select(a => a.Subscription).ToList());
            return View(data);
        }


        [Authorize(DashboardViewEnum.PromoCode, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            PromoCodeCreateOrEditModel model = new()
            {
                PromoCodeLang = new PromoCodeLangModel()
            };

            if (id > 0)
            {
                model = _mapper.Map<PromoCodeCreateOrEditModel>(
                                                await _unitOfWork.PromoCode.FindPromoCodebyId(id, trackChanges: false));

                model.Fk_Subscriptions = _unitOfWork.PromoCode.GetPromoCodeSubscriptions(new PromoCodeSubscriptionParameters
                {
                    Fk_PromoCode = id
                }, otherLang: false).Select(a => a.Fk_Subscription).ToList();
            }
            else
            {
                model.ExpirationDate = DateTime.UtcNow;
            }

      
            SetViewData();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.PromoCode, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, PromoCodeCreateOrEditModel model)
        {

            if (!ModelState.IsValid)
            {
                SetViewData();

                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                PromoCode dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<PromoCode>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.PromoCode.CreatePromoCode(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.PromoCode.FindPromoCodebyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;
                }


                await _unitOfWork.Save();

                await _unitOfWork.PromoCode.UpdatePromoCodeSubscriptions(dataDB.Id, model.Fk_Subscriptions);

                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData();

            return View(model);
        }

        [Authorize(DashboardViewEnum.PromoCode, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            PromoCode data = await _unitOfWork.PromoCode.FindPromoCodebyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.PromoCode, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.PromoCode.DeletePromoCode(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData()
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ViewData["Subscription"] = _unitOfWork.Subscription.GetSubscriptionsLookUp(new SubscriptionParameters(), otherLang);

        }

    }
}
