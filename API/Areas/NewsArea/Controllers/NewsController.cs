using API.Controllers;
using Entities.CoreServicesModels.NewsModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static Contracts.EnumData.DBModelsEnum;

namespace API.Areas.NewsArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("News")]
    [ApiExplorerSettings(GroupName = "News")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class NewsController : ExtendControllerBase
    {
        public NewsController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetNews))]
        public async Task<IEnumerable<NewsModel>> GetNews(
        [FromQuery] NewsParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];
            UserAuthenticatedDto auth = (UserAuthenticatedDto)Request.HttpContext.Items[ApiConstants.User];

            _365CompetitionsEnum _365CompetitionsEnum = (_365CompetitionsEnum)auth.Season._365_CompetitionsId.ParseToInt();

            parameters.OrderBy = "id desc";
            parameters.GetAttachments = true;
            parameters._365_CompetitionsId = (int)_365CompetitionsEnum;

            PagedList<NewsModel> data = await _unitOfWork.News.GetNewsPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpGet]
        [Route(nameof(GetNewsById))]
        public NewsModel GetNewsById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            NewsModel data = _unitOfWork.News.GetNewsbyId(id, otherLang);

            data.NewsAttachments = _unitOfWork.News.GetNewsAttachments(new NewsAttachmentParameters
            {
                Fk_News = id
            }).ToList();

            return data;
        }
    }
}
