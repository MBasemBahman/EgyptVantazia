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
                           parameters.Fk_Season,
                           parameters.Fk_Mark,
                           parameters.Fk_Teams,
                           parameters.Fk_Players);
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
            int fk_Season,
            int fk_Mark,
            List<int> fk_Teams,
            List<int> fk_Players)
        {
            return PlayerMarks.Where(a => (id == 0 || a.Id == id) &&
                                    (fk_Player == 0 || a.Fk_Player == fk_Player) &&
                                    (fk_Season == 0 || a.Player.Team.Fk_Season == fk_Season) &&
                                    (fk_Mark == 0 || a.Fk_Mark == fk_Mark) &&
                                    (fk_Teams == null || !fk_Teams.Any() || fk_Teams.Contains(a.Player.Fk_Team)) &&
                                    (fk_Players == null || !fk_Players.Any() || fk_Players.Contains(a.Fk_Player)) );
        }

    }
}
