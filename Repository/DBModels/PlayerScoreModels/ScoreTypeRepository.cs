using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.DBModels.PlayerScoreModels;

namespace Repository.DBModels.PlayerScoreModels
{
    public class ScoreTypeRepository : RepositoryBase<ScoreType>
    {
        public ScoreTypeRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<ScoreType> FindAll(ScoreTypeParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters._365_TypeId);
        }

        public async Task<ScoreType> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.ScoreTypeLang)
                        .SingleOrDefaultAsync();
        }

        public new void Create(ScoreType entity)
        {
            if (!FindByCondition(a => a.Name == entity.Name, trackChanges: false).Any())
            {
                entity.ScoreTypeLang ??= new ScoreTypeLang
                {
                    Name = entity.Name,
                };
                base.Create(entity);
            }
        }
    }

    public static class ScoreTypeRepositoryExtension
    {
        public static IQueryable<ScoreType> Filter(
            this IQueryable<ScoreType> ScoreTypes,
            int id,
            string _365_TypeId)
        {
            return ScoreTypes.Where(a => (id == 0 || a.Id == id) &&
                                         (string.IsNullOrWhiteSpace(_365_TypeId) || a._365_TypeId == _365_TypeId));
        }

    }
}
