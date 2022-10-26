using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerScoreModels;
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
                           parameters.Fk_GameWeak,
                           parameters.Fk_PlayerPosition,
                           parameters._365_PlayerId,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo,
                           parameters._365_PlayerIds,
                           parameters.Fk_TeamGameWeak_Ignored,
                           parameters.Fk_Players);
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
            if (entity._365_PlayerId.IsExisting() && FindByCondition(a => a._365_PlayerId == entity._365_PlayerId, trackChanges: false).Any())
            {
                Player oldEntity = FindByCondition(a => a._365_PlayerId == entity._365_PlayerId, trackChanges: false)
                                .Include(a => a.PlayerLang)
                                .First();

                oldEntity.Name = entity.Name;
                oldEntity._365_PlayerId = entity._365_PlayerId;
                oldEntity.ShortName = entity.ShortName;
                oldEntity.Age = entity.Age;
                oldEntity.PlayerNumber = entity.PlayerNumber;
                //oldEntity.Fk_PlayerPosition = entity.Fk_PlayerPosition;
                oldEntity.Fk_Team = entity.Fk_Team;
                oldEntity.PlayerLang.Name = entity.PlayerLang.Name;
                oldEntity.PlayerLang.ShortName = entity.PlayerLang.ShortName;
            }
            else
            {
                entity.PlayerLang ??= new PlayerLang
                {
                    Name = entity.Name,
                };
                base.Create(entity);
            }
        }
    }

    public static class PlayerRepositoryExtension
    {
        public static IQueryable<Player> Filter(
            this IQueryable<Player> Players,
            int id,
            int Fk_Team,
            int Fk_GameWeak,
            int Fk_PlayerPosition,
            string _365_PlayerId,
            DateTime? createdAtFrom,
            DateTime? createdAtTo,
            List<string> _365_PlayerIds,
            int fk_TeamGameWeak_Ignored,
            List<int> fk_Players)

        {
            return Players.Where(a => (id == 0 || a.Id == id) &&
                                      (Fk_Team == 0 || a.Fk_Team == Fk_Team) &&
                                      (Fk_GameWeak == 0 || a.PlayerGameWeaks.Select(a => a.Fk_TeamGameWeak)
                                          .Contains(Fk_GameWeak)) &&
                                      (createdAtFrom == null || a.CreatedAt >= createdAtFrom) &&
                                      (createdAtTo == null || a.CreatedAt <= createdAtTo) &&
                                      (fk_TeamGameWeak_Ignored == 0 || !a.PlayerGameWeaks.Any(b => b.Fk_TeamGameWeak == fk_TeamGameWeak_Ignored)) &&
                                      (Fk_PlayerPosition == 0 || a.Fk_PlayerPosition == Fk_PlayerPosition) &&
                                      (_365_PlayerIds == null || !_365_PlayerIds.Any() || _365_PlayerIds.Contains(a._365_PlayerId)) &&
                                      (string.IsNullOrWhiteSpace(_365_PlayerId) || a._365_PlayerId == _365_PlayerId) &&
                                      (fk_Players == null || !fk_Players.Any() || fk_Players.Contains(a.Id)));

        }
    }
}
