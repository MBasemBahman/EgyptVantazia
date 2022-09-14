using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.TeamModels
{
    public class PlayerRepository : RepositoryBase<Player>
    {
        public PlayerRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Player> FindAll(PlayerParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Team,
                           parameters.Fk_PlayerPosition);
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
        public static IQueryable<Player> Filter(
            this IQueryable<Player> Players,
            int id,
            int Fk_Team,
            int Fk_PlayerPosition)

        {
            return Players.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_Team == 0 || a.Fk_Team == Fk_Team) &&
                                                   (Fk_PlayerPosition == 0 || a.Fk_PlayerPosition == Fk_PlayerPosition));

        }

    }
}
