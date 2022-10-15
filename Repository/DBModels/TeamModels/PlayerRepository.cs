using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;

namespace Repository.DBModels.TeamModels
{
    public class PlayerRepository : RepositoryBase<Player>
    {
        public PlayerRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<Player> FindAll(PlayerParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Team,
                           parameters.Fk_PlayerPosition,
                           parameters._365_PlayerId,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo);
                           parameters._365_PlayerIds,
                           parameters.Fk_GameWeak_Ignored);
        }

        public async Task<Player> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.PlayerLang)
                        .SingleOrDefaultAsync();
        }

        public async Task<Player> FindBy365Id(string id, bool trackChanges)
        {
            return await FindByCondition(a => a._365_PlayerId == id, trackChanges)
                        .Include(a => a.PlayerLang)
                        .SingleOrDefaultAsync();
        }

        public new void Create(Player entity)
        {
            entity.PlayerLang ??= new PlayerLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }
    }

    public static class PlayerRepositoryExtension
    {
        public static IQueryable<Player> Filter(
            this IQueryable<Player> Players,
            int id,
            int Fk_Team,
            int Fk_PlayerPosition,
            string _365_PlayerId,
            DateTime? createdAtFrom,
            DateTime? createdAtTo)
            List<string> _365_PlayerIds,
            int fk_GameWeak_Ignored)

        {
            return Players.Where(a => (id == 0 || a.Id == id) &&
                                      (Fk_Team == 0 || a.Fk_Team == Fk_Team) &&
                                      (createdAtFrom == null || a.CreatedAt >= createdAtFrom)&&
                                      (createdAtTo == null || a.CreatedAt <= createdAtTo) &&
                                      (fk_GameWeak_Ignored == 0 || !a.PlayerGameWeaks.Any(b => b.Fk_GameWeak == fk_GameWeak_Ignored)) &&
                                      (Fk_PlayerPosition == 0 || a.Fk_PlayerPosition == Fk_PlayerPosition) &&
                                      (_365_PlayerIds == null || !_365_PlayerIds.Any() || _365_PlayerIds.Contains(a._365_PlayerId)) &&
                                      (string.IsNullOrWhiteSpace(_365_PlayerId) || a._365_PlayerId == _365_PlayerId));

        }
    }
}
