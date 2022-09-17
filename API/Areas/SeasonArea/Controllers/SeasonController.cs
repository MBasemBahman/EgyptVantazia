using API.Areas.SeasonArea.Models;
using API.Controllers;
using Entities.CoreServicesModels.SeasonModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Areas.SeasonArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Season")]
    [ApiExplorerSettings(GroupName = "Season")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class SeasonController : ExtendControllerBase
    {
        public SeasonController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetSeasons))]
        public async Task<IEnumerable<SeasonDto>> GetSeasons(
        [FromQuery] SeasonParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<SeasonModel> data = await _unitOfWork.Season.GetSeasonPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            IEnumerable<SeasonDto> dataDto = _mapper.Map<IEnumerable<SeasonDto>>(data);

            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetSeasonById))]
        public SeasonDto GetSeasonById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SeasonModel data = _unitOfWork.Season.GetSeasonbyId(id, otherLang);

            SeasonDto dataDto = _mapper.Map<SeasonDto>(data);

            return dataDto;
        }
    }
}
