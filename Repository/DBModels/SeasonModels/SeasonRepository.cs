using Entities.DBModels.SeasonModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.SeasonModels
{
    public class SeasonRepository : RepositoryBase<Season>
    {
        public SeasonRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Season> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<Season> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.SeasonLang)
                        .SingleOrDefaultAsync();
        }

        public new void Create(Season entity)
        {
            entity.SeasonLang ??= new SeasonLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }
    }

    public static class SeasonRepositoryExtension
    {
        public static IQueryable<Season> Filter(this IQueryable<Season> Seasons, int id)
        {
            return Seasons.Where(a => id == 0 || a.Id == id);
        }

    }
}
