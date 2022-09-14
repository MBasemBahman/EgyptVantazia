using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;

namespace Repository.DBModels.SeasonModels
{
    public class GameWeakRepository : RepositoryBase<GameWeak>
    {
        public GameWeakRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<GameWeak> FindAll(GameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Season);
        }

        public async Task<GameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(GameWeak entity)
        {
            entity.GameWeakLang ??= new GameWeakLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }
    }

    public static class GameWeakRepositoryExtension
    {
        public static IQueryable<GameWeak> Filter(
            this IQueryable<GameWeak> GameWeaks,
            int id,
            int Fk_Season
            )
        {
            return GameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                        (Fk_Season == 0 || a.Fk_Season == Fk_Season));

        }

    }
}
