using Dashboard.Areas.DashboardAdministration.Models;
using Entities.DBModels.DashboardAdministrationModels;
using Entities.RequestFeatures;
namespace Dashboard.Areas.DashboardAdministration.Controllers
{
    [Area("DashboardAdministration")]
    [Authorize(DashboardViewEnum.DashboardAdministrationRole, AccessLevelEnum.View)]
    public class DashboardAdministrationRoleController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public DashboardAdministrationRoleController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int id)
        {
            DashboardAdministrationRoleFilter filter = new()
            {
                Id = id
            };

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] DashboardAdministrationRoleFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            DashboardAdministrationRoleRequestParameters parameters = new()
            {
                SearchColumns = "Id,Name"
            };

            _ = _mapper.Map(dtParameters, parameters);

            parameters.GetDeveloperRole = int.Parse(Request.Cookies[AdminCookiesDataConstants.Role]) == (int)DashboardAdministrationRoleEnum.Developer;

            PagedList<DashboardAdministrationRoleModel> data = await _unitOfWork.DashboardAdministration.GetRolesPaged(parameters, otherLang);

            List<DashboardAdministrationRoleDto> resultDto = _mapper.Map<List<DashboardAdministrationRoleDto>>(data);

            DataTable<DashboardAdministrationRoleDto> dataTableManager = new();

            DataTableResult<DashboardAdministrationRoleDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.DashboardAdministration.GetRolesCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }


        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];


            DashboardAdministrationRoleDetailsViewModel model = new()
            {
                Role = _mapper.Map<DashboardAdministrationRoleDto>(_unitOfWork.DashboardAdministration
                                                            .GetRolebyId(id, otherLang))
            };


            foreach (DashboardAccessLevelModel accesslevel in _unitOfWork.DashboardAdministration.GetAccessLevels(
                new DashboardAccessLevelParameters { Fk_DashboardAdministrationRole = id }, otherLang).ToList())
            {
                model.Permissions.Add(accesslevel.Name
                    , _unitOfWork.DashboardAdministration.GetPremissions(
                        new AdministrationRolePremissionParameters
                        {
                            Fk_DashboardAccessLevel = accesslevel.Id,
                            Fk_DashboardAdministrationRole = id
                        }, otherLang: false)
                    .Select(b => b.DashboardView.Name).ToList());
            }

            return View(model);
        }

        [Authorize(DashboardViewEnum.DashboardAccessLevel, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            DashboardAdministrationRoleCreateOrEditModel model = new()
            {
                DashboardAdministrationRoleLang = new()
            };

            if (id > 0)
            {
                model = _mapper.Map<DashboardAdministrationRoleCreateOrEditModel>(
                                                await _unitOfWork.DashboardAdministration.FindRoleById(id, trackChanges: false));
            }

            SetViewData(otherLang);
            return View(_unitOfWork.DashboardAdministration.GetRoleCreateOrEditViewModel(model, null, otherLang, id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.DashboardAccessLevel, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(
            int id,
            DashboardAdministrationRoleCreateOrEditModel Role,
            List<RolePermissionCreateOrEditViewModel> permissions = null)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (!ModelState.IsValid)
            {
                SetViewData(otherLang);
                return View(_unitOfWork.DashboardAdministration.GetRoleCreateOrEditViewModel(Role, permissions, otherLang, id));
            }
            try
            {
                DashboardAdministrationRole dataDb = new();
                if (id == 0)
                {

                    dataDb = _mapper.Map<DashboardAdministrationRole>(Role);
                    _unitOfWork.DashboardAdministration.CreateRole(dataDb, permissions);
                }
                else
                {
                    dataDb = await _unitOfWork.DashboardAdministration.FindRoleById(id, trackChanges: true);
                    _ = _mapper.Map(Role, dataDb);

                    _unitOfWork.DashboardAdministration.UpdateRolePermissions(id, permissions);
                }

                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(otherLang);

            return View(_unitOfWork.DashboardAdministration.GetRoleCreateOrEditViewModel(Role, permissions, otherLang, id));
        }

        [Authorize(DashboardViewEnum.DashboardAccessLevel, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            DashboardAdministrationRole data = await _unitOfWork.DashboardAdministration.FindRoleById(id, trackChanges: false);

            return View(data != null &&
                !_unitOfWork.DashboardAdministration
                .GetAdministrators(new DashboardAdministratorParameters
                { Fk_DashboardAdministrationRole = id }, otherLang: false).Any());
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.DashboardAccessLevel, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.DashboardAdministration.DeleteRole(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        private void SetViewData(bool otherLang)
        {
            ViewData["SystemViews"] = _unitOfWork.DashboardAdministration.GetViewsLookUp(new DashboardViewParameters(), otherLang);
        }
    }
}
