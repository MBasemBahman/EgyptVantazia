using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.DBModels.PlayerMarkModels;

namespace Repository.DBModels.PlayerMarkModels
{
    public class PlayerMarkRepository : RepositoryBase<PlayerMark>
    {
        public PlayerMarkRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<PlayerMark> FindAll(PlayerMarkParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Player,
                           parameters.Fk_Mark,
                           parameters.Fk_GameWeak);
        }

        public async Task<PlayerMark> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(PlayerMark entity)
        {
            base.Create(entity);
        }
    }

    public static class PlayerMarkRepositoryExtension
    {
        public static IQueryable<PlayerMark> Filter(
            this IQueryable<PlayerMark> PlayerMarks,
            int id,
            int fk_Player,
            int fk_Mark,
            int fk_GameWeak)
        {
            return PlayerMarks.Where(a => (id == 0 || a.Id == id) &&
                                    (fk_Player == 0 || a.Fk_Player == fk_Player) &&
                                    (fk_Mark == 0 || a.Fk_Mark == fk_Mark) &&
                                    (fk_GameWeak == 0 || a.PlayerMarkGameWeaks.Any(b => b.Fk_GameWeak == fk_GameWeak) ) );
        }

    }
}
