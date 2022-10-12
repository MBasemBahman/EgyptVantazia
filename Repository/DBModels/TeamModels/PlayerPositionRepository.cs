using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;

namespace Repository.DBModels.TeamModels
{
    public class PlayerPositionRepository : RepositoryBase<PlayerPosition>
    {
        public PlayerPositionRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PlayerPosition> FindAll(PlayerPositionParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters._365_PositionId);
        }

        public async Task<PlayerPosition> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.PlayerPositionLang)
                        .SingleOrDefaultAsync();
        }

        public async Task<PlayerPosition> FindBy365Id(string id, bool trackChanges)
        {
            return await FindByCondition(a => a._365_PositionId == id, trackChanges)
                        .Include(a => a.PlayerPositionLang)
                        .SingleOrDefaultAsync();
        }

        public new void Create(PlayerPosition entity)
        {
            entity.PlayerPositionLang ??= new PlayerPositionLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }
    }

    public static class PlayerPositionRepositoryExtension
    {
        public static IQueryable<PlayerPosition> Filter(
            this IQueryable<PlayerPosition> PlayerPositions,
            int id,
            string _365_PositionId)
        {
            return PlayerPositions.Where(a => (id == 0 || a.Id == id) &&
                                              (string.IsNullOrWhiteSpace(_365_PositionId) || a._365_PositionId == _365_PositionId));
        }

    }
}
