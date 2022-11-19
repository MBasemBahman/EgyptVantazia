using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;


namespace Repository.DBModels.SeasonModels
{
    public class SeasonRepository : RepositoryBase<Season>
    {
        public SeasonRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Season> FindAll(SeasonParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters._365_SeasonId,
                           parameters.IsCurrent);
        }

        public async Task<Season> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.SeasonLang)
                        .FirstOrDefaultAsync();
        }

        public async Task<Season> FindBy365Id(string id, bool trackChanges)
        {
            return await FindByCondition(a => a._365_SeasonId == id, trackChanges)
                        .Include(a => a.SeasonLang)
                        .FirstOrDefaultAsync();
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
        public static IQueryable<Season> Filter(
            this IQueryable<Season> Seasons,
            int id,
            string _365_SeasonId,
            bool? isCurrent)
        {
            return Seasons.Where(a => (id == 0 || a.Id == id) &&
                                      (isCurrent == null || a.IsCurrent == isCurrent) &&
                                      (string.IsNullOrWhiteSpace(_365_SeasonId) || a._365_SeasonId == _365_SeasonId));
        }

    }
}
