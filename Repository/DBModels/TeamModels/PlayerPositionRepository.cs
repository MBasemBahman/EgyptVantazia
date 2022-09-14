using Entities.DBModels.TeamModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.TeamModels
{
    public class PlayerPositionRepository : RepositoryBase<PlayerPosition>
    {
        public PlayerPositionRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<PlayerPosition> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<PlayerPosition> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
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
        public static IQueryable<PlayerPosition> Filter(this IQueryable<PlayerPosition> PlayerPositions, int id)
        {
            return PlayerPositions.Where(a => id == 0 || a.Id == id);
        }

    }
}
