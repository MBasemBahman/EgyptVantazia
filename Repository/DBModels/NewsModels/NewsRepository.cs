using Entities.CoreServicesModels.NewsModels;
using Entities.DBModels.NewsModels;
using static Entities.EnumData.LogicEnumData;

namespace Repository.DBModels.NewsModels
{
    public class NewsRepository : RepositoryBase<News>
    {
        public NewsRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<News> FindAll(NewsParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_GameWeak,
                           parameters.Fk_Season,
                           parameters.NewsTypeEnum,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo);
        }

        public async Task<News> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.NewsLang)
                        .FirstOrDefaultAsync();
        }

        public new void Create(News entity)
        {
            entity.NewsLang ??= new NewsLang
            {
                Title = entity.Title,
                ShortDescription = entity.ShortDescription,
                LongDescription = entity.LongDescription,
            };
            base.Create(entity);
        }
    }

    public static class NewsRepositoryExtension
    {
        public static IQueryable<News> Filter(
            this IQueryable<News> Newss,
            int id,
            int Fk_GameWeak,
            int Fk_Season,
            NewsTypeEnum NewsTypeEnum,
            DateTime? createdAtFrom,
            DateTime? createdAtTo)
        {
            return Newss.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_GameWeak == 0 || a.Fk_GameWeak == Fk_GameWeak) &&
                                                   (NewsTypeEnum == 0 || a.NewsTypeEnum == NewsTypeEnum) &&
                                                   (Fk_Season == 0 || a.GameWeak.Fk_Season == Fk_Season) &&
                                                   (createdAtFrom == null || a.CreatedAt >= createdAtFrom) &&
                                                   (createdAtTo == null || a.CreatedAt <= createdAtTo));

        }

    }
}
