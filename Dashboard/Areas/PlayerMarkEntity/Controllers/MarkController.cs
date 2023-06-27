using Dashboard.Areas.PlayerMarkEntity.Models;
using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerMarkModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.PlayerMarkEntity.Controllers
{
    [Area("PlayerMarkEntity")]
    [Authorize(DashboardViewEnum.Mark, AccessLevelEnum.View)]
    public class MarkController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public MarkController(ILoggerManager logger, IMapper mapper,
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
            MarkFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] MarkFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            MarkParameters parameters = new()
            {
                SearchColumns = "Id,Name",
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<MarkModel> data = await _unitOfWork.PlayerMark.GetMarkPaged(parameters, otherLang);

            List<MarkDto> resultDto = _mapper.Map<List<MarkDto>>(data);

            DataTable<MarkDto> dataTableManager = new();

            DataTableResult<MarkDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.PlayerMark.GetMarkCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            MarkDto data = _mapper.Map<MarkDto>(_unitOfWork.PlayerMark
                                                           .GetMarkbyId(id, otherLang));

            return View(data);
        }


        [Authorize(DashboardViewEnum.Mark, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            MarkCreateOrEditModel model = new()
            {
                MarkLang = new()
            };

            if (id > 0)
            {
                var markDb = await _unitOfWork.PlayerMark.FindMarkbyId(id, trackChanges: false);
                
                model = _mapper.Map<MarkCreateOrEditModel>(markDb);
                
                model.ImageUrl = markDb.StorageUrl + markDb.ImageUrl;
            }

            SetViewData();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.Mark, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, MarkCreateOrEditModel model)
        {

            if (!ModelState.IsValid)
            {
                SetViewData();
                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                Mark dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<Mark>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.PlayerMark.CreateMark(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.PlayerMark.FindMarkbyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;
                }

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];

                if (imageFile != null)
                {
                    dataDB.ImageUrl = await _unitOfWork.PlayerMark.UploudMark(_environment.WebRootPath, imageFile);
                    dataDB.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
                }

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

        [Authorize(DashboardViewEnum.Mark, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            Mark data = await _unitOfWork.PlayerMark.FindMarkbyId(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.Mark, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.PlayerMark.DeleteMark(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }


        public void SetViewData()
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ViewData["Marks"] = _unitOfWork.PlayerMark.GetMarksLookUp(new MarkParameters(), otherLang);
        }
    }
}
