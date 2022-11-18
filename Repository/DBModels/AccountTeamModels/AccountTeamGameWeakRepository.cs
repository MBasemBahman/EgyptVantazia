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
                           parameters.Fk_GameWeak,
                           parameters.Fk_Season,
                           parameters.Fk_Account,
                           parameters._365_GameWeakId);

        }

        public async Task<AccountTeamGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }

        public double GetAverageGameWeakPoints(int fk_GameWeak)
        {
            return FindByCondition(a => a.Fk_GameWeak == fk_GameWeak, trackChanges: false)
                   .Select(a => a.TotalPoints)
                   .Average();
        }
    }

    public static class AccountTeamGameWeakRepositoryExtension
    {
        public static IQueryable<AccountTeamGameWeak> Filter(
            this IQueryable<AccountTeamGameWeak> AccountTeamGameWeaks,
            int id,
            int Fk_AccountTeam,
            int Fk_GameWeak,
            int Fk_Season,
            int Fk_Account,
            string _365_GameWeakId)

        {
            return AccountTeamGameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                                   (Fk_AccountTeam == 0 || a.Fk_AccountTeam == Fk_AccountTeam) &&
                                                   (string.IsNullOrEmpty(_365_GameWeakId) || a.GameWeak._365_GameWeakId == _365_GameWeakId) &&
                                                   (Fk_Season == 0 || a.AccountTeam.Fk_Season == Fk_Season) &&
                                                   (Fk_Account == 0 || a.AccountTeam.Fk_Account == Fk_Account) &&
                                                   (Fk_GameWeak == 0 || a.Fk_GameWeak == Fk_GameWeak));

        }

    }
}
