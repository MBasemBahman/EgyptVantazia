using Dashboard.Areas.Location.Models;
using Entities.CoreServicesModels.LocationModels;
using Entities.DBModels.LocationModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.Location.Controllers
{
    [Area("Location")]
    [Authorize(DashboardViewEnum.Country, AccessLevelEnum.View)]
    public class CountryController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public CountryController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int id)
        {
            CountryFilter filter = new()
            {
                Id = id
            };

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            return View(filter);
        }
        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] CountryFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            RequestParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<CountryModel> data = await _unitOfWork.Location.GetCountrysPaged(parameters, otherLang);

            List<CountryDto> resultDto = _mapper.Map<List<CountryDto>>(data);

            DataTable<CountryDto> dataTableManager = new();

            DataTableResult<CountryDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Location.GetCountryCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }


        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            CountryDto data = _mapper.Map<CountryDto>(_unitOfWork.Location
                                                            .GetCountrybyId(id, otherLang));

            return View(data);
        }

        [Authorize(DashboardViewEnum.Country, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            CountryCreateOrEditModel model = new()
            {
                CountryLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<CountryCreateOrEditModel>(
                                                await _unitOfWork.Location.FindCountrybyId(id, trackChanges: false));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.Country, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, CountryCreateOrEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                Country dataDb = new();

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

                if (id == 0)
                {

                    dataDb = _mapper.Map<Country>(model);

                    dataDb.CreatedBy = auth.UserName;

                    _unitOfWork.Location.CreateCountry(dataDb);
                }
                else
                {
                    dataDb = await _unitOfWork.Location.FindCountrybyId(id, trackChanges: true);

                    dataDb.LastModifiedBy = auth.UserName;

                    _ = _mapper.Map(model, dataDb);
                }

                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            return View(model);
        }

        [Authorize(DashboardViewEnum.Country, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            Country data = await _unitOfWork.Location.FindCountrybyId(id, trackChanges: false);

            return View(data!=null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.Country, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Location.DeleteCountry(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
