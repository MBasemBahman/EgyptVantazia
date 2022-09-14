using Entities.CoreServicesModels.NewsModels;
using Entities.DBModels.NewsModels;


namespace Repository.DBModels.NewsModels
{
    public class NewsRepository : RepositoryBase<News>
    {
        public NewsRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<News> FindAll(NewsParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_GameWeak);
        }

        public async Task<News> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.NewsLang)
                        .SingleOrDefaultAsync();
        }

        public new void Create(News entity)
        {
            base.Create(entity);
        }

        public new void Delete(News entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class NewsRepositoryExtension
    {
        public static IQueryable<News> Filter(
            this IQueryable<News> Newss,
            int id,
            int? Fk_GameWeak)
        {
            return Newss.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_GameWeak == null || a.Fk_GameWeak == Fk_GameWeak));

        }

    }
}
