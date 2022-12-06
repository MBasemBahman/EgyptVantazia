using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;

namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamPlayerRepository : RepositoryBase<AccountTeamPlayer>
    {
        public AccountTeamPlayerRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<AccountTeamPlayer> FindAll(AccountTeamPlayerParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Account,
                           parameters.Fk_AccountTeam,
                           parameters.Fk_Player,
                           parameters.Fk_Season,
                           parameters.Fk_GameWeak,
                           parameters.IsCurrent,
                           parameters.IsNextGameWeak,
                           parameters.IsTransfer);
        }

        public async Task<AccountTeamPlayer> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(AccountTeamPlayer entity)
        {
            if (!FindByCondition(a => a.Fk_AccountTeam == entity.Fk_AccountTeam && a.Fk_Player == entity.Fk_Player, trackChanges: false).Any())
            {
                base.Create(entity);
            }
        }
    }

    public static class AccountTeamPlayerRepositoryExtension
    {
        public static IQueryable<AccountTeamPlayer> Filter(
            this IQueryable<AccountTeamPlayer> AccountTeamPlayers,
            int id,
            int Fk_Account,
            int Fk_AccountTeam,
            int Fk_Player,
            int Fk_Season,
            int Fk_GameWeak,
            bool? IsCurrent,
            bool? IsNextGameWeak,
            bool? IsTransfer)
        {
            return AccountTeamPlayers.Where(a => (id == 0 || a.Id == id) &&
                                                 (IsTransfer == null ||
                                                  Fk_GameWeak == 0 ||
                                                  a.AccountTeamPlayerGameWeaks
                                                   .Any(b => b.Fk_GameWeak == Fk_GameWeak && 
                                                             b.AccountTeamPlayer.Fk_AccountTeam == Fk_AccountTeam &&
                                                             b.IsTransfer == IsTransfer)) &&
                                                 (Fk_GameWeak == 0 || a.AccountTeamPlayerGameWeaks.Any(b => b.Fk_GameWeak == Fk_GameWeak)) &&
                                                 (IsCurrent == null || (IsCurrent == true ? a.AccountTeamPlayerGameWeaks.Any(b => b.GameWeak.IsCurrent) : !a.AccountTeamPlayerGameWeaks.Any(b => b.GameWeak.IsCurrent))) &&
                                                 (IsNextGameWeak == null || (IsNextGameWeak == true ? a.AccountTeamPlayerGameWeaks.Any(b => b.GameWeak.IsNext) : !a.AccountTeamPlayerGameWeaks.Any(b => b.GameWeak.IsNext))) &&
                                                 (Fk_Account == 0 || a.AccountTeam.Fk_Account == Fk_Account) &&
                                                 (Fk_AccountTeam == 0 || a.Fk_AccountTeam == Fk_AccountTeam) &&
                                                 (Fk_Season == 0 || a.AccountTeam.Fk_Season == Fk_Season) &&
                                                 (Fk_Player == 0 || a.Fk_Player == Fk_Player));

        }

    }
}
