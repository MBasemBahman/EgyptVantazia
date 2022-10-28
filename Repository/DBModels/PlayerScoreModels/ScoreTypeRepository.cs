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
                           parameters._365_TypeId,
                           parameters.Ids,
                           parameters.IsEvent,
                           parameters.HavePoints);
        }

        public async Task<ScoreType> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.ScoreTypeLang)
                        .SingleOrDefaultAsync();
        }

        public new void Create(ScoreType entity)
        {
            if (FindByCondition(a => a._365_TypeId == entity._365_TypeId && a._365_EventTypeId == entity._365_EventTypeId, trackChanges: false).Any())
            {
                ScoreType oldEntity = FindByCondition(a => a._365_TypeId == entity._365_TypeId, trackChanges: false)
                                .Include(a => a.ScoreTypeLang)
                                .First();

                oldEntity.Name = entity.Name;
                oldEntity._365_TypeId = entity._365_TypeId;
                oldEntity.IsEvent = entity.IsEvent;
                oldEntity._365_EventTypeId = entity._365_EventTypeId;
                oldEntity.ScoreTypeLang.Name = entity.ScoreTypeLang.Name;
            }
            else
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
            string _365_TypeId,
            List<int> ids,
            bool? isEvent,
            bool? havePoints)
        {
            return ScoreTypes.Where(a => (id == 0 || a.Id == id) &&
                                         (string.IsNullOrWhiteSpace(_365_TypeId) || a._365_TypeId == _365_TypeId) &&
                                         (ids == null || !ids.Any() || ids.Contains(a.Id)) &&
                                         (isEvent == null || a.IsEvent == isEvent) &&
                                         (havePoints == null || a.HavePoints == havePoints));
        }

    }
}
