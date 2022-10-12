using Dashboard.Areas.DashboardAdministration.Models;
using Entities.CoreServicesModels.UserModels;
using Entities.DBModels.DashboardAdministrationModels;
using Entities.RequestFeatures;
namespace Dashboard.Areas.DashboardAdministration.Controllers
{
    [Area("DashboardAdministration")]
    [Authorize(DashboardViewEnum.DashboardAdministrator, AccessLevelEnum.View)]
    public class DashboardAdministratorController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public DashboardAdministratorController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int id, int fk_DashboardAdministrationRole)
        {
            DashboardAdministratorFilter filter = new()
            {
                Id = id,
                Fk_DashboardAdministrationRole = fk_DashboardAdministrationRole,
            };

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] DashboardAdministratorFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            DashboardAdministratorParameters parameters = new()
            {
                SearchColumns = "Id,JobTitle"
            };

            _ = _mapper.Map(dtParameters, parameters);

            parameters.GetDevelopers = int.Parse(Request.Cookies[AdminCookiesDataConstants.Role]) == (int)DashboardAdministrationRoleEnum.Developer;

            PagedList<DashboardAdministratorModel> data = await _unitOfWork.DashboardAdministration.GetAdministratorsPaged(parameters, otherLang);

            List<DashboardAdministratorDto> resultDto = _mapper.Map<List<DashboardAdministratorDto>>(data);

            DataTable<DashboardAdministratorDto> dataTableManager = new();

            DataTableResult<DashboardAdministratorDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.DashboardAdministration.GetAdministratorsCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            DashboardAdministratorDto data = _mapper.Map<DashboardAdministratorDto>(_unitOfWork.DashboardAdministration
                                                           .GetAdministratorbyId(id, otherLang));

            return View(data);
        }

        [Authorize(DashboardViewEnum.DashboardAdministrator, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            DashboardAdministratorCreateOrEditModelDto model = new();

            if (id > 0)
            {
                DashboardAdministrator adminDB = await _unitOfWork.DashboardAdministration.FindAdministratorById(id, trackChanges: false);
                model.DashboardAdministrator = _mapper.Map<DashboardAdministratorCreateOrEditModel>(adminDB);
                model.User = _mapper.Map<UserCreateModel>(await _unitOfWork.User.FindByAdminId(id, trackChanges: false));
            }

            SetViewData(otherLang);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.DashboardAdministrator, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, DashboardAdministratorCreateOrEditModelDto model)
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
                DashboardAdministrator adminDB = new();
                if (id == 0)
                {
                    adminDB = _mapper.Map<DashboardAdministrator>(model.DashboardAdministrator);

                    adminDB.CreatedBy = auth.UserName;

                    adminDB.User = new();

                    _ = _mapper.Map(model.User, adminDB.User);
                    adminDB.User.CreatedBy = auth.UserName;

                    _unitOfWork.DashboardAdministration.CreateAdministrator(adminDB);

                }
                else
                {
                    adminDB = await _unitOfWork.DashboardAdministration.FindAdministratorById(id, trackChanges: true);
                    User userDB = await _unitOfWork.User.FindByAdminId(id, trackChanges: true);

                    if (model.User.Password != userDB.Password)
                    {
                        model.User.Password = _unitOfWork.User.ChangePassword(model.User.Password);
                    }

                    _ = _mapper.Map(model.User, userDB);
                    _ = _mapper.Map(model.DashboardAdministrator, adminDB);

                    userDB.LastModifiedBy = auth.UserName;
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

        [Authorize(DashboardViewEnum.DashboardAdministrator, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            DashboardAdministrator data = await _unitOfWork.DashboardAdministration.FindAdministratorById(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.DashboardAdministrator, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.DashboardAdministration.DeleteAdministrator(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(bool otherLang)
        {
            ViewData["Roles"] = _unitOfWork.DashboardAdministration.GetRolesLookUp(new DashboardAdministrationRoleRequestParameters()
            { GetDeveloperRole = false }, otherLang);


        }
    }
}
