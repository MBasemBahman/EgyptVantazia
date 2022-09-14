using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;
using Entities.RequestFeatures;

namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamPlayerRepository : RepositoryBase<AccountTeamPlayer>
    {
        public AccountTeamPlayerRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<AccountTeamPlayer> FindAll(AccountTeamPlayerParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_AccountTeam,
                           parameters.Fk_Player);
        }

        public async Task<AccountTeamPlayer> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public new void Create(AccountTeamPlayer entity)
        {
            base.Create(entity);
        }

        public new void Delete(AccountTeamPlayer entity)
        {
            base.Delete(entity);
        }


        public new int Count()
        {
            return base.Count();
        }
    }

    public static class AccountTeamPlayerRepositoryExtension
    {
        public static IQueryable<AccountTeamPlayer> Filter(
            this IQueryable<AccountTeamPlayer> AccountTeamPlayers,
            int id,
            int Fk_AccountTeam,
            int Fk_Player)
        { 
            return AccountTeamPlayers.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_AccountTeam == 0 || a.Fk_AccountTeam == Fk_AccountTeam) &&
                                                   (Fk_Player == 0 || a.Fk_Player == Fk_Player));

        }

    }
}
