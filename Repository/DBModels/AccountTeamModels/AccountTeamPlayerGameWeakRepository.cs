using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;

namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamPlayerGameWeakRepository : RepositoryBase<AccountTeamPlayerGameWeak>
    {
        public AccountTeamPlayerGameWeakRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<AccountTeamPlayerGameWeak> FindAll(AccountTeamPlayerGameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_AccountTeamPlayer,
                           parameters.Fk_TeamPlayerType,
                           parameters.Fk_Player,
                           parameters.Fk_AccountTeam,
                           parameters.Fk_Account,
                           parameters.Fk_Season);
        }

        public async Task<AccountTeamPlayerGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }
    }

    public static class AccountTeamPlayerGameWeakRepositoryExtension
    {
        public static IQueryable<AccountTeamPlayerGameWeak> Filter(
            this IQueryable<AccountTeamPlayerGameWeak> AccountTeamPlayerGameWeaks,
            int id,
            int Fk_AccountTeamPlayer,
            int Fk_TeamPlayerType,
            int Fk_Player,
            int Fk_AccountTeam,
            int Fk_Account,
            int Fk_Season)

        {
            return AccountTeamPlayerGameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_AccountTeamPlayer == 0 || a.Fk_AccountTeamPlayer == Fk_AccountTeamPlayer) &&
                                                   (Fk_Account == 0 || a.AccountTeamPlayer.AccountTeam.Fk_Account == Fk_Account) &&
                                                   (Fk_Season == 0 || a.AccountTeamPlayer.AccountTeam.Fk_Account == Fk_Season) &&
                                                   (Fk_Player == 0 || a.AccountTeamPlayer.Fk_Player == Fk_Player) &&
                                                   (Fk_AccountTeam == 0 || a.AccountTeamPlayer.Fk_AccountTeam == Fk_AccountTeam) &&
                                                   (Fk_TeamPlayerType == 0 || a.Fk_TeamPlayerType == Fk_TeamPlayerType));


        }

    }
}
