using Entities.DBModels.PrivateLeagueModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.PrivateLeagueModels
{
    public class PrivateLeagueRepository : RepositoryBase<PrivateLeague>
    {
        public PrivateLeagueRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PrivateLeague> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<PrivateLeague> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.PrivateLeagueLang)
                        .SingleOrDefaultAsync();
        }

        public new void Create(PrivateLeague entity)
        {
            entity.PrivateLeagueLang ??= new PrivateLeagueLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }

        public new void Delete(PrivateLeague entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class PrivateLeagueRepositoryExtension
    {
        public static IQueryable<PrivateLeague> Filter(this IQueryable<PrivateLeague> PrivateLeagues, int id)
        {
            return PrivateLeagues.Where(a => id == 0 || a.Id == id);
        }

    }
}
