using Entities.CoreServicesModels.LocationModels;
using Entities.DBModels.LocationModels;


namespace CoreServices.Services
{
    public class LocationServices
    {
        private readonly RepositoryManager _repository;

        public LocationServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region Country Services
        public IQueryable<CountryModel> GetCountrys(RequestParameters parameters,
                bool otherLang)
        {
            return _repository.Country
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new CountryModel
                       {
                           Name = otherLang ? a.CountryLang.Name : a.Name,
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<CountryModel>> GetCountrysPaged(
                  RequestParameters parameters,
                  bool otherLang)
        {
            return await PagedList<CountryModel>.ToPagedList(GetCountrys(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public Dictionary<string, string> GetCountrysLookUp(RequestParameters parameters, bool otherLang)
        {
            return GetCountrys(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }

        public async Task<Country> FindCountrybyId(int id, bool trackChanges)
        {
            return await _repository.Country.FindById(id, trackChanges);
        }

        public void CreateCountry(Country Country)
        {
            _repository.Country.Create(Country);
        }

        public async Task DeleteCountry(int id)
        {
            Country Country = await FindCountrybyId(id, trackChanges: true);
            _repository.Country.Delete(Country);
        }

        public CountryModel GetCountrybyId(int id, bool otherLang)
        {
            return GetCountrys(new RequestParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetCountryCount()
        {
            return _repository.Country.Count();
        }
        #endregion

    }
}
