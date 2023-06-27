using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.DBModels.PlayerMarkModels;

namespace Repository.DBModels.PlayerMarkModels
{
    public class PlayerMarkReasonMatchRepository : RepositoryBase<PlayerMarkReasonMatch>
    {
        public PlayerMarkReasonMatchRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<PlayerMarkReasonMatch> FindAll(PlayerMarkReasonMatchParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_PlayerMark,
                           parameters.Fk_TeamGameWeak,
                           parameters.Fk_TeamGameWeaks);
        }

        public async Task<PlayerMarkReasonMatch> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(PlayerMarkReasonMatch entity)
        {
            base.Create(entity);
        }
    }

    public static class PlayerMarkReasonMatchRepositoryExtension
    {
        public static IQueryable<PlayerMarkReasonMatch> Filter(
            this IQueryable<PlayerMarkReasonMatch> PlayerMarkReasonMatchs,
            int id,
            int fk_PlayerMark,
            int fk_TeamGameWeak,
            List<int> fk_TeamGameWeaks)
        {
            return PlayerMarkReasonMatchs.Where(a => (id == 0 || a.Id == id) &&
                                    (fk_PlayerMark == 0 || a.Fk_PlayerMark == fk_PlayerMark) &&
                                    (fk_TeamGameWeak == 0 || a.Fk_TeamGameWeak == fk_TeamGameWeak) &&
                                    (fk_TeamGameWeaks == null || !fk_TeamGameWeaks.Any() || fk_TeamGameWeaks.Contains(a.Fk_TeamGameWeak) ) );
        }

    }
}
