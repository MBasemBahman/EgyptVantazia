using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;

namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamGameWeakRepository : RepositoryBase<AccountTeamGameWeak>
    {
        public AccountTeamGameWeakRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<AccountTeamGameWeak> FindAll(AccountTeamGameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_AccountTeam,
                           parameters.Fk_GameWeak);

        }

        public async Task<AccountTeamGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }
    }

    public static class AccountTeamGameWeakRepositoryExtension
    {
        public static IQueryable<AccountTeamGameWeak> Filter(
            this IQueryable<AccountTeamGameWeak> AccountTeamGameWeaks,
            int id,
            int Fk_AccountTeam,
            int Fk_GameWeak

            )

        {
            return AccountTeamGameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_AccountTeam == 0 || a.Fk_AccountTeam == Fk_AccountTeam) &&
                                                   (Fk_GameWeak == 0 || a.Fk_GameWeak == Fk_GameWeak));

        }

    }
}
