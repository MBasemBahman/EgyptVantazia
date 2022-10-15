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
                           parameters.Fk_TeamGameWeak,
                           parameters.Fk_Player);
        }

        public async Task<PlayerGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(PlayerGameWeak entity)
        {
            if (FindByCondition(a => a.Fk_TeamGameWeak == entity.Fk_TeamGameWeak && a.Fk_Player == entity.Fk_Player, trackChanges: false).Any())
            {
                PlayerGameWeak oldEntity = FindByCondition(a => a.Fk_TeamGameWeak == entity.Fk_TeamGameWeak && a.Fk_Player == entity.Fk_Player, trackChanges: false).First();

                oldEntity.Ranking = entity.Ranking;
            }
            else
            {
                base.Create(entity);
            }
        }
    }

    public static class PlayerGameWeakRepositoryExtension
    {
        public static IQueryable<PlayerGameWeak> Filter(
            this IQueryable<PlayerGameWeak> PlayerGameWeaks,
            int id,
            int Fk_TeamGameWeak,
            int Fk_Player)
        {
            return PlayerGameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                              (Fk_TeamGameWeak == 0 || a.Fk_TeamGameWeak == Fk_TeamGameWeak) &&
                                              (Fk_Player == 0 || a.Fk_Player == Fk_Player));


        }

    }
}
