using API.Controllers;
using Entities.CoreServicesModels.UserModels;

namespace API.Areas.UserArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Authentication")]
    [ApiExplorerSettings(GroupName = "Authentication")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class DeviceController : ExtendControllerBase
    {
        private readonly IFirebaseNotificationManager _notification;

        public DeviceController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IFirebaseNotificationManager notification,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        {
            _notification = notification;
        }

        [HttpPost]
        [Route(nameof(CreateUserDevice))]
        public async Task<bool> CreateUserDevice(
            [FromBody] DeviceCreateModel model)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            Device device = _mapper.Map<Device>(model);
            device.Fk_User = auth.Id;

            _unitOfWork.User.CreateDevice(device);
            await _unitOfWork.Save();
            return true;
        }

        [HttpDelete]
        [Route(nameof(DeleteUserDevices))]
        public async Task<bool> DeleteUserDevices()
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            IEnumerable<Device> devices = await _unitOfWork.User.FindDevicesByUserId(auth.Id, trackChanges: true);

            foreach (Device device in devices)
            {
                _unitOfWork.User.DeleteDevice(device);
            }

            await _unitOfWork.Save();

            return true;
        }
    }
}
