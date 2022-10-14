using Dashboard.Areas.UserEntity.Models;
using Entities.CoreServicesModels.UserModels;
using Entities.RequestFeatures;

namespace Dashboard.Areas.UserEntity.Controllers
{
    [Area("UserEntity")]
    [Authorize(DashboardViewEnum.User, AccessLevelEnum.View)]
    public class UserController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public UserController(ILoggerManager logger, IMapper mapper,
                UnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int id, int Fk_Governerate, int Fk_EnterpriseType, int Fk_Enterprise)
        {
            UserFilter filter = new()
            {
                Id = id,
             
            };

            ViewData[ViewDataConstants.AccessLevel] = (DashboardAccessLevelModel)Request.HttpContext.Items[ViewDataConstants.AccessLevel];

            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] UserFilter dtParameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            UserParameters parameters = new()
            {
                SearchColumns = "Id,Account,UserName,PhoneNumber,Culture"
            };

            _ = _mapper.Map(dtParameters, parameters);

            PagedList<UserModel> data = await _unitOfWork.User.GetUsersPaged(parameters, otherLang);

            List<UserDto> resultDto = _mapper.Map<List<UserDto>>(data);

            DataTable<UserDto> dataTableManager = new();

            DataTableResult<UserDto> dataTableResult = dataTableManager.LoadTable(dtParameters, resultDto, data.MetaData.TotalCount, _unitOfWork.User.GetUsersCount());

            return Json(dataTableManager.ReturnTable(dataTableResult));
        }

        public IActionResult Details(int id)
        {
            UserDto data = _mapper.Map<UserDto>(_unitOfWork.User
                                                           .GetUserbyId(id, trackChanges: false));

            return View(data);
        }

        public IActionResult Profile(int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            UserDto data = _mapper.Map<UserDto>(_unitOfWork.User
                                                           .GetUserbyId(id, trackChanges: false));

            ViewData["otherLang"] = otherLang;

            return View(data);
        }

        [Authorize(DashboardViewEnum.User, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id = 0, bool IsProfile = false)
        {
            UserCreateModel model = _mapper.Map<UserCreateModel>(await _unitOfWork.User.FindById(id, trackChanges: false));

            SetViewData(IsProfile, id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(DashboardViewEnum.User, AccessLevelEnum.CreateOrEdit)]
        public async Task<IActionResult> CreateOrEdit(int id, UserCreateModel model, bool IsProfile = false)
        {
            if (!ModelState.IsValid)
            {
                SetViewData(IsProfile, id);

                return View(model);
            }
            try
            {
                UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

                User dataDb = await _unitOfWork.User.FindById(id, trackChanges: true);

                if (model.Password != dataDb.Password)
                {
                    model.Password = _unitOfWork.User.ChangePassword(model.Password);
                }
                _ = _mapper.Map(model, dataDb);


                dataDb.LastModifiedBy = auth.UserName;

                await _unitOfWork.Save();

                return IsProfile ? RedirectToAction(nameof(Profile), new { id }) : (IActionResult)RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData[ViewDataConstants.Error] = _logger.LogError(HttpContext.Request, ex).ErrorMessage;
            }

            SetViewData(IsProfile, id);

            return View(model);
        }

        [Authorize(DashboardViewEnum.User, AccessLevelEnum.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            User data = await _unitOfWork.User.FindById(id, trackChanges: false);

            return View(data != null);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(DashboardViewEnum.User, AccessLevelEnum.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.User.DeleteUser(id);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // helper methods
        private void SetViewData(bool IsProfile, int id)
        {
            ViewData["IsProfile"] = IsProfile;
            ViewData["id"] = id;
        }
    }
}
