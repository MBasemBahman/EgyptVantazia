using Dashboard.Areas.TeamEntity.Models;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.TeamEntity.Controllers
{
    [Area("TeamEntity")]
    [Authorize(DashboardViewEnum.FormationPosition, AccessLevelEnum.View)]
    public class FormationPositionController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly LinkGenerator _linkGenerator;
        private readonly IWebHostEnvironment _environment;

        public FormationPositionController(ILoggerManager logger, IMapper mapper,
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

            FormationPositionFilter filter = new();

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] FormationPositionFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            FormationPositionParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<FormationPositionModel> data = await _unitOfWork.Team.GetFormationPositionPaged(parameters, otherLang);

            List<FormationPositionDto> resultDto = _mapper.Map<List<FormationPositionDto>>(data);

            DataTable<FormationPositionDto> dataTableManager = new();

            DataTableResult<FormationPositionDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.Team.GetFormationPositionCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            FormationPositionDto data = _mapper.Map<FormationPositionDto>(_unitOfWork.Team
                                                           .GetFormationPositionbyId(id, otherLang));

            return View(data);
        }


        [Authorize(DashboardViewEnum.FormationPosition, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            FormationPositionCreateOrEditModel model = new()
            {
                FormationPositionLang = new(),
            };

            if (id > 0)
            {
                model = _mapper.Map<FormationPositionCreateOrEditModel>(
                                                await _unitOfWork.Team.FindFormationPositionbyId(id, trackChanges: false));
            }

            if (model.ImageUrl.IsNullOrEmpty())
            {
                model.ImageUrl = "football-team.png";
                model.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.FormationPosition, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, FormationPositionCreateOrEditModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {

                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];
                FormationPosition dataDB = new();
                if (id == 0)
                {
                    dataDB = _mapper.Map<FormationPosition>(model);

                    dataDB.CreatedBy = auth.UserName;

                    _unitOfWork.Team.CreateFormationPosition(dataDB);

                }
                else
                {
                    dataDB = await _unitOfWork.Team.FindFormationPositionbyId(id, trackChanges: true);

                    _ = _mapper.Map(model, dataDB);

                    dataDB.LastModifiedBy = auth.UserName;
                }

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];

                if (imageFile != null)
                {
                    dataDB.ImageUrl = await _unitOfWork.Team.UploudFormationPositionImage(_environment.WebRootPath, imageFile);
                    dataDB.StorageUrl = _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
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

        [Authorize(DashboardViewEnum.FormationPosition, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            FormationPosition data = await _unitOfWork.Team.FindFormationPositionbyId(id, trackChanges: false);

            return View(data != null && !_unitOfWork.Team.GetPlayers(new PlayerParameters
            {
                Fk_FormationPosition = id
            }, otherLang: false).Any());
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.FormationPosition, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.Team.DeleteFormationPosition(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }


    }
}
