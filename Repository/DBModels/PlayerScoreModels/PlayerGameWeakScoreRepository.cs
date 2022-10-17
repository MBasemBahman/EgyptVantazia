using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.DBModels.PlayerScoreModels;


namespace Repository.DBModels.PlayerScoreModels
{
    public class PlayerGameWeakScoreRepository : RepositoryBase<PlayerGameWeakScore>
    {
        public PlayerGameWeakScoreRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PlayerGameWeakScore> FindAll(PlayerGameWeakScoreParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_PlayerGameWeak,
                           parameters.Fk_ScoreType,
                           parameters.Fk_Player,
                           parameters.Fk_GameWeak,
                           parameters.FinalValueFrom,
                           parameters.FinalValueTo);
        }

        public async Task<PlayerGameWeakScore> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(PlayerGameWeakScore entity)
        {
            if (FindByCondition(a => a.Fk_PlayerGameWeak == entity.Fk_PlayerGameWeak && a.Fk_ScoreType == entity.Fk_ScoreType, trackChanges: false).Any())
            {
                PlayerGameWeakScore oldEntity = FindByCondition(a => a.Fk_PlayerGameWeak == entity.Fk_PlayerGameWeak && a.Fk_ScoreType == entity.Fk_ScoreType, trackChanges: true).First();

                oldEntity.Value = entity.Value;
                oldEntity.FinalValue = entity.FinalValue;
                oldEntity.GameTime = entity.GameTime;
                oldEntity.Points = entity.Points;
            }
            else
            {
                base.Create(entity);
            }
        }
    }

    public static class PlayerGameWeakScoreRepositoryExtension
    {
        public static IQueryable<PlayerGameWeakScore> Filter
            (this IQueryable<PlayerGameWeakScore> PlayerGameWeakScores,
             int id,
             int fk_PlayerGameWeak,
             int fk_ScoreType,
             int fk_Player,
             int fk_GameWeak,
             int? finalValueFrom,
             int? finalValueTo)

        {
            return PlayerGameWeakScores.Where(a => (id == 0 || a.Id == id) &&
                                                   (finalValueFrom == null || a.FinalValue >= finalValueFrom) &&
                                                   (finalValueTo == null || a.FinalValue <= finalValueTo) &&
                                                   (fk_PlayerGameWeak == 0 || a.Fk_PlayerGameWeak == fk_PlayerGameWeak) &&
                                                   (fk_Player == 0 || a.PlayerGameWeak.Fk_Player == fk_Player) &&
                                                   (fk_GameWeak == 0 || a.PlayerGameWeak.TeamGameWeak.Fk_GameWeak == fk_GameWeak) &&
                                                   (fk_ScoreType == 0 || a.Fk_ScoreType == fk_ScoreType));

        }

    }
}
