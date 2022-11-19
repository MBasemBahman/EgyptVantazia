using Entities.DBModels.LocationModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.LocationModels
{
    public class CountryRepository : RepositoryBase<Country>
    {
        public CountryRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Country> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<Country> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.CountryLang)
                        .FirstOrDefaultAsync();
        }

        public new void Create(Country entity)
        {
            entity.CountryLang ??= new CountryLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }
    }

    public static class CountryRepositoryExtension
    {
        public static IQueryable<Country> Filter(this IQueryable<Country> Countrys, int id)
        {
            return Countrys.Where(a => id == 0 || a.Id == id);
        }

    }
}
