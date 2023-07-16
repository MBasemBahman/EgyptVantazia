using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;

namespace Repository.DBModels.TeamModels
{
    public class FormationPositionRepository : RepositoryBase<FormationPosition>
    {
        public FormationPositionRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<FormationPosition> FindAll(FormationPositionParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters._365_PositionId);
        }

        public async Task<FormationPosition> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.FormationPositionLang)
                        .FirstOrDefaultAsync();
        }

        public async Task<FormationPosition> FindBy365Id(string id, bool trackChanges)
        {
            return await FindByCondition(a => a._365_PositionId == id, trackChanges)
                        .Include(a => a.FormationPositionLang)
                        .FirstOrDefaultAsync();
        }

        public new void Create(FormationPosition entity)
        {
            if (entity.Name.IsEmpty())
            {
                return;
            }

            if (entity._365_PositionId.IsExisting() && FindByCondition(a => a._365_PositionId == entity._365_PositionId, trackChanges: false).Any())
            {
                FormationPosition oldEntity = FindByCondition(a => a._365_PositionId == entity._365_PositionId, trackChanges: true)
                                .Include(a => a.FormationPositionLang)
                                .First();

                oldEntity.Name = entity.Name;
                oldEntity._365_PositionId = entity._365_PositionId;
                oldEntity.FormationPositionLang.Name = entity.FormationPositionLang.Name;
            }
            else
            {
                entity.FormationPositionLang ??= new FormationPositionLang
                {
                    Name = entity.Name,
                };
                base.Create(entity);
            }
        }
    }

    public static class FormationPositionRepositoryExtension
    {
        public static IQueryable<FormationPosition> Filter(
            this IQueryable<FormationPosition> FormationPositions,
            int id,
            string _365_PositionId)
        {
            return FormationPositions.Where(a => (id == 0 || a.Id == id) &&
                                              (string.IsNullOrWhiteSpace(_365_PositionId) || a._365_PositionId == _365_PositionId));
        }

    }
}
