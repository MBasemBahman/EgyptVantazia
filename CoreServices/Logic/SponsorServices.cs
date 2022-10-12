using Entities.CoreServicesModels.SponsorModels;
using Entities.DBModels.SponsorModels;
using static Entities.EnumData.LogicEnumData;

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


        public async Task<string> UploudSponsorImage(string rootPath, IFormFile file)
        {
            FileUploader uploader = new(rootPath);
            return await uploader.UploudFile(file, "Uploud/Sponsor");
        }
        public void CreateSponsor(Sponsor Sponsor, List<AppViewEnum> views)
        {
            Sponsor = AddSponsorViews(Sponsor, views);
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
                           AppViewEnum = a.AppViewEnum,
                           Id = a.Id,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public Sponsor AddSponsorViews(Sponsor entity, List<AppViewEnum> views)
        {
            if (views != null && views.Any())
            {
                entity.SponsorViews = new List<SponsorView>();

                foreach (AppViewEnum view in views)
                {
                    entity.SponsorViews.Add(new SponsorView
                    {
                        AppViewEnum = view
                    });
                }
            }
            return entity;
        }
        public void UpdateSponsorViews(int fk_sponsor, List<AppViewEnum> newData)
        {

            List<AppViewEnum> oldData = GetSponsorViews(new SponsorViewParameters
            { Fk_Sponsor = fk_sponsor }).Select(a => a.AppViewEnum).ToList();

            List<AppViewEnum> addedData = newData.Except(oldData).ToList();
            List<AppViewEnum> removedData = oldData.Except(newData).ToList();

            AddSponsorViews(fk_sponsor, addedData);
            RemoveSponsorViews(fk_sponsor, removedData);

        }

        private void AddSponsorViews(int fk_sponsor, List<AppViewEnum> views)
        {
            foreach (AppViewEnum view in views)
            {
                SponsorView data = new()
                {
                    Fk_Sponsor = fk_sponsor,
                    AppViewEnum = view,
                };

                _repository.SponsorView.Create(data);
            }
        }

        private void RemoveSponsorViews(int fk_sponspor, List<AppViewEnum> views)
        {
            foreach (AppViewEnum view in views)
            {
                SponsorView data =
                    _repository.SponsorView.FindAll(new SponsorViewParameters
                    {
                        AppView = (int)view,
                        Fk_Sponsor = fk_sponspor
                    }, trackChanges: false).SingleOrDefault();

                _repository.SponsorView.Delete(data);
            }
        }



        #endregion

    }
}
