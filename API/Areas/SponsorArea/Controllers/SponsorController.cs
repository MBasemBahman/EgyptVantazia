using API.Controllers;
using Entities.CoreServicesModels.LocationModels;
using Entities.CoreServicesModels.SponsorModels;
using Entities.DBModels.SponsorModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Areas.SponsorArea.Controllers
{
    [ApiVersion("1.0")]
    [Area("Sponsor")]
    [ApiExplorerSettings(GroupName = "Sponsor")]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    public class SponsorController : ExtendControllerBase
    {
        public SponsorController(
        ILoggerManager logger,
        IMapper mapper,
        UnitOfWork unitOfWork,
        LinkGenerator linkGenerator,
        IWebHostEnvironment environment,
        IOptions<AppSettings> appSettings) : base(logger, mapper, unitOfWork, linkGenerator, environment, appSettings)
        { }

        [HttpGet]
        [Route(nameof(GetSponsor))]
        public async Task<IEnumerable<SponsorModel>> GetSponsor(
        [FromQuery] SponsorParameters parameters)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            PagedList<SponsorModel> data = await _unitOfWork.Sponsor.GetSponsorPaged(parameters, otherLang);

            SetPagination(data.MetaData, parameters);

            return data;
        }

        [HttpGet]
        [Route(nameof(GetSponsorById))]
        public SponsorModel GetSponsorById(
        [FromQuery, BindRequired] int id)
        {
            bool otherLang = (bool)Request.HttpContext.Items[ApiConstants.Language];

            SponsorModel data = _unitOfWork.Sponsor.GetSponsorbyId(id, otherLang);

            data.SponsorViews = _unitOfWork.Sponsor.GetSponsorViews(new SponsorViewParameters
            {
                Fk_Sponsor = id
            }).Select(a => a.AppViewEnum)
              .ToList();

            return data;
        }
    }
}
