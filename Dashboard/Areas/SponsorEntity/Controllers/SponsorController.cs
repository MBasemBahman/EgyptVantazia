using Dashboard.Areas.SponsorEntity.Models;
using Entities.CoreServicesModels.SponsorModels;
using Entities.DBModels.SponsorModels;
using Entities.RequestFeatures;
using static Entities.EnumData.LogicEnumData;

namespace Dashboard.Areas.SponsorEntity.Controllers
{
    [Area("SponsorEntity")]
    [Authorize(DashboardViewEnum.Sponsor, AccessLevelEnum.View)]
    public class SponsorController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public SponsorController(ILoggerManager logger, IMapper mapper,
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

            SponsorFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] SponsorFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SponsorParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<SponsorModel> data = await _unitOfWork.Sponsor.GetSponsorPaged(parameters, otherLang);

            List<SponsorDto> resultDto = _mapper.Map<List<SponsorDto>>(data);

            DataTable<SponsorDto> dataTableManager = new();

            DataTableResult<SponsorDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Sponsor.GetSponsorCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SponsorDto data = _mapper.Map<SponsorDto>(_unitOfWork.Sponsor
                                                           .GetSponsors(new SponsorParameters
                                                           {
                                                               Id = id,
                                                               GetViews = true
                                                           }, otherLang).FirstOrDefault());




            return View(data);
        }

        [Authorize(DashboardViewEnum.Sponsor, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SponsorCreateOrEditModel model = new()
            {
                SponsorLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<SponsorCreateOrEditModel>(
                                                await _unitOfWork.Sponsor.FindSponsorbyId(id, trackChanges: false));

                model.SponsorViews = _unitOfWork.Sponsor.GetSponsorViews(new SponsorViewParameters
                {
                    Fk_Sponsor = id
                }).ToList().Any()
                ? _unitOfWork.Sponsor.GetSponsorViews(new SponsorViewParameters
                {
                    Fk_Sponsor = id
                }).ToList().Select(a => a.AppViewEnum).ToList()
                : new List<AppViewEnum>();
            }

            if (model.ImageUrl.IsNullOrEmpty())
            {
                model.ImageUrl = "deals.png";
                model.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
            }


            SetViewData(otherLang);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.Sponsor, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, SponsorCreateOrEditModel model)
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
                Sponsor dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<Sponsor>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.Sponsor.CreateSponsor(dataDB, model.SponsorViews);

                }
                else
                {
                    dataDB = await _unitOfWork.Sponsor.FindSponsorbyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;

                    _unitOfWork.Sponsor.UpdateSponsorViews(id, model.SponsorViews);

                }

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];

                if (imageFile != null)
                {
                    dataDB.ImageUrl = await _unitOfWork.Sponsor.UploudSponsorImage(_environment.WebRootPath, imageFile);
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

        [Authorize(DashboardViewEnum.Sponsor, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            Sponsor data = await _unitOfWork.Sponsor.FindSponsorbyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.Sponsor, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Sponsor.DeleteSponsor(id);
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
