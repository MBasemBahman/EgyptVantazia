using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;

namespace Repository.DBModels.TeamModels
{
    public class PlayerPriceRepository : RepositoryBase<PlayerPrice>
    {
        public PlayerPriceRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PlayerPrice> FindAll(PlayerPriceParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Team,
                           parameters.Fk_Player);
        }

        public async Task<PlayerPrice> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(PlayerPrice entity)
        {
            base.Create(entity);
        }
    }

    public static class PlayerPriceRepositoryExtension
    {
        public static IQueryable<PlayerPrice> Filter(
            this IQueryable<PlayerPrice> PlayerPrices,
            int id,
            int Fk_Team,
            int Fk_Player)

        {
            return PlayerPrices.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_Team == 0 || a.Fk_Team == Fk_Team) &&
                                                   (Fk_Player == 0 || a.Fk_Player == Fk_Player));

        }

    }
}
