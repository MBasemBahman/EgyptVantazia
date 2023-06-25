using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.DBModels.PlayerMarkModels;

namespace Repository.DBModels.PlayerMarkModels
{
    public class PlayerMarkGameWeakRepository : RepositoryBase<PlayerMarkGameWeak>
    {
        public PlayerMarkGameWeakRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<PlayerMarkGameWeak> FindAll(PlayerMarkGameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_PlayerMark,
                           parameters.Fk_GameWeak,
                           parameters.Fk_GameWeaks);
        }

        public async Task<PlayerMarkGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(PlayerMarkGameWeak entity)
        {
            base.Create(entity);
        }
    }

    public static class PlayerMarkGameWeakRepositoryExtension
    {
        public static IQueryable<PlayerMarkGameWeak> Filter(
            this IQueryable<PlayerMarkGameWeak> PlayerMarkGameWeaks,
            int id,
            int fk_PlayerMark,
            int fk_GameWeak,
            List<int> fk_GameWeaks)
        {
            return PlayerMarkGameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                    (fk_PlayerMark == 0 || a.Fk_PlayerMark == fk_PlayerMark) &&
                                    (fk_GameWeak == 0 || a.Fk_GameWeak == fk_GameWeak) &&
                                    (fk_GameWeaks == null || !fk_GameWeaks.Any() || fk_GameWeaks.Contains(a.Fk_GameWeak) ) );
        }

    }
}
