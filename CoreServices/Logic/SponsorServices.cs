using Entities.CoreServicesModels.SponsorModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SponsorModels;

namespace CoreServices.Logic
{
    public class SponsorServices
    {
        private readonly RepositoryManager _repository;

        public SponsorServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region Sponsor Services
        public IQueryable<SponsorModel> GetSponsors(SponsorParameters parameters,
                bool otherLang)
        {
            return _repository.Sponsor
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new SponsorModel
                       {
                           Name = otherLang ? a.SponsorLang.Name : a.Name,
                           ImageUrl = a.StorageUrl + a.ImageUrl,
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           SponsorViewsCount = a.SponsorViews.Count,
                           SponsorViews = parameters.GetViews ? a.SponsorViews
                                                                  .Select(b => b.AppViewEnum)
                                                                  .ToList() : null
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<SponsorModel>> GetSponsorPaged(
                  SponsorParameters parameters,
                  bool otherLang)
        {
            return await PagedList<SponsorModel>.ToPagedList(GetSponsors(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Sponsor> FindSponsorbyId(int id, bool trackChanges)
        {
            return await _repository.Sponsor.FindById(id, trackChanges);
        }

        public void CreateSponsor(Sponsor Sponsor)
        {
            _repository.Sponsor.Create(Sponsor);
        }

        public async Task DeleteSponsor(int id)
        {
            Sponsor Sponsor = await FindSponsorbyId(id, trackChanges: true);
            _repository.Sponsor.Delete(Sponsor);
        }

        public SponsorModel GetSponsorbyId(int id, bool otherLang)
        {
            return GetSponsors(new SponsorParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetSponsorCount()
        {
            return _repository.Sponsor.Count();
        }
        #endregion

        #region SponsorView Services
        public IQueryable<SponsorViewModel> GetSponsorViews(SponsorViewParameters parameters)
        {
            return _repository.SponsorView
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new SponsorViewModel
                       {
                           AppViewEnum = a.AppViewEnum
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }
        #endregion

    }
}
