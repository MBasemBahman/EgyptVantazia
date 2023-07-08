using API.Controllers;
using CoreServices;
using Entities.CoreServicesModels.AccountModels;
namespace API.Areas.AccountArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Account")]
    [ApiExplorerSettings(GroupName = "Account")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class AccountController : ExtendControllerBase
    {
        public AccountController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        {
        }

        [HttpGet]
        [Route(nameof(GetAccountById))]
        public AccountModel GetAccountById(
        [FromQuery] int id)
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            if (id == 0)
            {
                id = auth.Fk_Account;
            }

            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            AccountModel data = _unitOfWork.Account.GetAccountbyId(id, otherLang);

            return data;
        }

        [HttpDelete]
        [Route(nameof(DeleteAccount))]
        public bool DeleteAccount()
        {
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            _unitOfWork.Account.DeleteAccount(auth.Fk_Account).Wait();
            _unitOfWork.Save().Wait();

            return true;
        }
    }
}
