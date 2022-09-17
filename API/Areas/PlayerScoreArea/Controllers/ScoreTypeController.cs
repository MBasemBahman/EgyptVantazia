using API.Areas.PlayerScoreArea.Models;
using API.Controllers;
using Entities.CoreServicesModels.PlayerScoreModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Areas.PlayerScoreArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("PlayerScore")]
    [ApiExplorerSettings(GroupName = "PlayerScore")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class ScoreTypeController : ExtendControllerBase
    {
        public ScoreTypeController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetScoreTypes))]
        public async Task<IEnumerable<ScoreTypeDto>> GetScoreTypes(
        [FromQuery] ScoreTypeParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<ScoreTypeModel> data = await _unitOfWork.PlayerScore.GetScoreTypePaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            IEnumerable<ScoreTypeDto> dataDto = _mapper.Map<IEnumerable<ScoreTypeDto>>(data);

            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetScoreTypeById))]
        public ScoreTypeDto GetScoreTypeById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            ScoreTypeModel data = _unitOfWork.PlayerScore.GetScoreTypebyId(id, otherLang);

            ScoreTypeDto dataDto = _mapper.Map<ScoreTypeDto>(data);

            return dataDto;
        }
    }
}
