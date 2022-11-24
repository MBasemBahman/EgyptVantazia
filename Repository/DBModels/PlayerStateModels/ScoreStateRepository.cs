using Entities.CoreServicesModels.PlayerStateModels;
using Entities.DBModels.PlayerStateModels;


namespace Repository.DBModels.PlayerStateModels
{
    public class ScoreStateRepository : RepositoryBase<ScoreState>
    {
        public ScoreStateRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<ScoreState> FindAll(ScoreStateParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Ids,
                           parameters.Fk_GameWeak,
                           parameters.Fk_GameWeaks,
                           parameters.Fk_Season,
                           parameters.Fk_Seasons);
        }

        public async Task<ScoreState> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.ScoreStateLang)
                        .FirstOrDefaultAsync();
        }
        public new void Create(ScoreState entity)
        {
            entity.ScoreStateLang ??= new ScoreStateLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }

    }

    public static class ScoreStateRepositoryExtension
    {
        public static IQueryable<ScoreState> Filter(
            this IQueryable<ScoreState> ScoreStates,
            int id,
            List<int> ids,
            int fk_GameWeak,
            List<int> fk_GameWeaks,
            int fk_Season,
            List<int> fk_Seasons)
        {
            return ScoreStates.Where(a => (id == 0 || a.Id == id) &&
                                         (ids == null || !ids.Any() || ids.Contains(a.Id)) &&
                                         (fk_GameWeak == 0 || a.PlayerGameWeakScoreStates.Any(b => b.Fk_GameWeak == fk_GameWeak)) &&
                                         (fk_GameWeaks == null || !fk_GameWeaks.Any() ||
                                         a.PlayerGameWeakScoreStates.Any(b => fk_GameWeaks.Contains(b.Fk_GameWeak))) &&
                                         (fk_Season == 0 || a.PlayerSeasonScoreStates.Any(b => b.Fk_Season == fk_Season)) &&
                                         (fk_Seasons == null || !fk_Seasons.Any() ||
                                         a.PlayerSeasonScoreStates.Any(b => fk_Seasons.Contains(b.Fk_Season))));

        }

    }
}


