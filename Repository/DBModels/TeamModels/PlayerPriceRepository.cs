using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.TeamModels
{
    public class PlayerPriceRepository : RepositoryBase<PlayerPrice>
    {
        public PlayerPriceRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PlayerPrice> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<PlayerPrice> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(PlayerPrice entity)
        {
            base.Create(entity);
        }

        public new void Delete(PlayerPrice entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class PlayerPriceRepositoryExtension
    {
        public static IQueryable<PlayerPrice> Filter(this IQueryable<PlayerPrice> PlayerPrices, int id)
        {
            return PlayerPrices.Where(a => id == 0 || a.Id == id);
        }

    }
}
