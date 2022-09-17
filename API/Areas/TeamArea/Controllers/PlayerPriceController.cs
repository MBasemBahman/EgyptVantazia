using API.Areas.TeamArea.Models;
using API.Controllers;
using Entities.CoreServicesModels.TeamModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Areas.TeamArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Team")]
    [ApiExplorerSettings(GroupName = "Team")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class PlayerPriceController : ExtendControllerBase
    {
        public PlayerPriceController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetPlayerPrices))]
        public async Task<IEnumerable<PlayerPriceDto>> GetPlayerPrices(
        [FromQuery] PlayerPriceParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<PlayerPriceModel> data = await _unitOfWork.Team.GetPlayerPricePaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            IEnumerable<PlayerPriceDto> dataDto = _mapper.Map<IEnumerable<PlayerPriceDto>>(data);

            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetPlayerPriceById))]
        public PlayerPriceDto GetPlayerPriceById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerPriceModel data = _unitOfWork.Team.GetPlayerPricebyId(id, otherLang);

            PlayerPriceDto dataDto = _mapper.Map<PlayerPriceDto>(data);

            return dataDto;
        }
    }
}
