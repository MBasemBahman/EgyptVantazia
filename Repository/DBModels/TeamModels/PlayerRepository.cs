using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.TeamModels
{
    public class PlayerRepository : RepositoryBase<Player>
    {
        public PlayerRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Player> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<Player> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(Player entity)
        {
            base.Create(entity);
        }

        public new void Delete(Player entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class PlayerRepositoryExtension
    {
        public static IQueryable<Player> Filter(this IQueryable<Player> Players, int id)
        {
            return Players.Where(a => id == 0 || a.Id == id);
        }

    }
}
