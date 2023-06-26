using API.Areas.PromoCodeArea.Models;
using API.Controllers;
using Entities.CoreServicesModels.PromoCodeModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Areas.PromoCodeArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Subscription")]
    [ApiExplorerSettings(GroupName = "PromoCode")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class PromoCodeController : ExtendControllerBase
    {
        public PromoCodeController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(CheckPromoCode))]
        public PromoCodeModel CheckPromoCode(
        [FromBody, BindRequired] CheckPromoCodeDto model)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            return model.Code.IsEmpty() || model.Fk_Subscription <= 0
                ? throw new Exception("Invalid code!")
                : _unitOfWork.PromoCode.CheckPromoCode(model.Code, model.Fk_Subscription, auth.Fk_Account, otherLang);
        }
    }
}
