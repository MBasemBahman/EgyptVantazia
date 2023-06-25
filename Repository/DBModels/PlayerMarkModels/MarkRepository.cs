using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.DBModels.PlayerMarkModels;
using Entities.DBModels.PlayerScoreModels;

namespace Repository.DBModels.PlayerMarkModels
{
    public class MarkRepository : RepositoryBase<Mark>
    {
        public MarkRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<Mark> FindAll(MarkParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Ids);
        }

        public async Task<Mark> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.MarkLang)
                        .FirstOrDefaultAsync();
        }

        public new void Create(Mark entity)
        {
            entity.MarkLang ??= new MarkLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }
    }

    public static class MarkRepositoryExtension
    {
        public static IQueryable<Mark> Filter(
            this IQueryable<Mark> Marks,
            int id,
            List<int> ids)
        {
            return Marks.Where(a => (id == 0 || a.Id == id) &&
                                    (ids == null || !ids.Any() || ids.Contains(a.Id)));
        }

    }
}
