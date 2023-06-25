using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.DBModels.PlayerMarkModels;

namespace Repository.DBModels.PlayerMarkModels
{
    public class PlayerMarkTeamGameWeakRepository : RepositoryBase<PlayerMarkTeamGameWeak>
    {
        public PlayerMarkTeamGameWeakRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<PlayerMarkTeamGameWeak> FindAll(PlayerMarkTeamGameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_PlayerMark,
                           parameters.Fk_TeamGameWeak,
                           parameters.Fk_TeamGameWeaks);
        }

        public async Task<PlayerMarkTeamGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(PlayerMarkTeamGameWeak entity)
        {
            base.Create(entity);
        }
    }

    public static class PlayerMarkTeamGameWeakRepositoryExtension
    {
        public static IQueryable<PlayerMarkTeamGameWeak> Filter(
            this IQueryable<PlayerMarkTeamGameWeak> PlayerMarkTeamGameWeaks,
            int id,
            int fk_PlayerMark,
            int fk_TeamGameWeak,
            List<int> fk_TeamGameWeaks)
        {
            return PlayerMarkTeamGameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                    (fk_PlayerMark == 0 || a.Fk_PlayerMark == fk_PlayerMark) &&
                                    (fk_TeamGameWeak == 0 || a.Fk_TeamGameWeak == fk_TeamGameWeak) &&
                                    (fk_TeamGameWeaks == null || !fk_TeamGameWeaks.Any() || fk_TeamGameWeaks.Contains(a.Fk_TeamGameWeak) ) );
        }

    }
}
