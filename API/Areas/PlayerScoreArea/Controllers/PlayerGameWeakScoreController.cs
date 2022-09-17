using API.Controllers;
using Entities.CoreServicesModels.PlayerScoreModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Areas.PlayerScoreArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("PlayerScore")]
    [ApiExplorerSettings(GroupName = "PlayerScore")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class PlayerGameWeakScoreController : ExtendControllerBase
    {
        public PlayerGameWeakScoreController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetPlayerGameWeakScores))]
        public async Task<IEnumerable<PlayerGameWeakScoreModel>> GetPlayerGameWeakScores(
        [FromQuery] PlayerGameWeakScoreParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<PlayerGameWeakScoreModel> data = await _unitOfWork.PlayerScore.GetPlayerGameWeakScorePaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpGet]
        [Route(nameof(GetPlayerGameWeakScoreById))]
        public PlayerGameWeakScoreModel GetPlayerGameWeakScoreById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PlayerGameWeakScoreModel data = _unitOfWork.PlayerScore.GetPlayerGameWeakScorebyId(id, otherLang);

            return data;
        }
    }
}
