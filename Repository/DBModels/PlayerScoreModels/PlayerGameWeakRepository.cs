using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.DBModels.PlayerScoreModels;


namespace Repository.DBModels.PlayerScoreModels
{
    public class PlayerGameWeakRepository : RepositoryBase<PlayerGameWeak>
    {
        public PlayerGameWeakRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PlayerGameWeak> FindAll(PlayerGameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_GameWeak,
                           parameters.Fk_Player);
        }

        public async Task<PlayerGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }
    }

    public static class PlayerGameWeakRepositoryExtension
    {
        public static IQueryable<PlayerGameWeak> Filter(
            this IQueryable<PlayerGameWeak> PlayerGameWeaks,
            int id,
            int Fk_GameWeak,
            int Fk_Player)
        {
            return PlayerGameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                              (Fk_GameWeak == 0 || a.Fk_GameWeak == Fk_GameWeak) &&
                                              (Fk_Player == 0 || a.Fk_Player == Fk_Player));


        }

    }
}
