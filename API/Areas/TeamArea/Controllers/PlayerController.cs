using API.Areas.TeamArea.Models;
using API.Controllers;
using Entities.CoreServicesModels.TeamModels;
using static Contracts.EnumData.DBModelsEnum;

namespace API.Areas.TeamArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Team")]
    [ApiExplorerSettings(GroupName = "Team")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class PlayerController : ExtendControllerBase
    {
        public PlayerController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetPlayers))]
        public async Task<IEnumerable<PlayerDto>> GetPlayers(
            [FromQuery] _365CompetitionsEnum _365CompetitionsEnum,
            [FromQuery] PlayerParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (parameters.IncludeScore && parameters.Fk_SeasonForScores == 0)
            {
                if (parameters.Fk_GameWeakForScores == 0)
                {
                    parameters.Fk_SeasonForScores = _unitOfWork.Season.GetCurrentSeasonId(_365CompetitionsEnum);
                    parameters.Fk_GameWeakForScores = _unitOfWork.Season.GetCurrentGameWeakId(_365CompetitionsEnum);
                }
            }

            parameters.IsActive = true;

            PagedList<PlayerModel> data = await _unitOfWork.Team.GetPlayerPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            IEnumerable<PlayerDto> dataDto = _mapper.Map<IEnumerable<PlayerDto>>(data);

            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetPlayerById))]
        public PlayerDto GetPlayerById(
            [FromQuery] _365CompetitionsEnum _365CompetitionsEnum,
            [FromQuery] PlayerParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            if (parameters.IncludeScore && parameters.Fk_SeasonForScores == 0)
            {
                if (parameters.Fk_GameWeakForScores == 0)
                {
                    parameters.Fk_SeasonForScores = _unitOfWork.Season.GetCurrentSeasonId(_365CompetitionsEnum);
                    parameters.Fk_GameWeakForScores = _unitOfWork.Season.GetCurrentGameWeakId(_365CompetitionsEnum);
                }
            }

            PlayerModel data = _unitOfWork.Team.GetPlayers(parameters, otherLang).FirstOrDefault();

            PlayerDto dataDto = _mapper.Map<PlayerDto>(data);

            return dataDto;
        }

        [HttpGet]
        [Route(nameof(GetRandomTeam))]
        public IEnumerable<PlayerDto> GetRandomTeam([FromQuery] _365CompetitionsEnum _365CompetitionsEnum)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            int fk_Season = _unitOfWork.Season.GetCurrentSeasonId(_365CompetitionsEnum);

            List<PlayerModel> data = _unitOfWork.Team.GetRandomTeam(fk_Season, isTop_11: false, otherLang);

            IEnumerable<PlayerDto> dataDto = _mapper.Map<IEnumerable<PlayerDto>>(data);

            return dataDto;
        }
    }
}
