using Entities.DBModels.StandingsModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.StandingsModels
{
    public class StandingsRepository : RepositoryBase<Standings>
    {
        public StandingsRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Standings> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<Standings> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(Standings entity)
        {
            base.Create(entity);
        }

        public new void Delete(Standings entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class StandingsRepositoryExtension
    {
        public static IQueryable<Standings> Filter(this IQueryable<Standings> Standingss, int id)
        {
            return Standingss.Where(a => id == 0 || a.Id == id);
        }

    }
}
