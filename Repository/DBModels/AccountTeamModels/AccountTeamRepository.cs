using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;


namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamRepository : RepositoryBase<AccountTeam>
    {
        public AccountTeamRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<AccountTeam> FindAll(AccountTeamParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Account,
                           parameters.Fk_Season);
        }

        public async Task<AccountTeam> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .SingleOrDefaultAsync();
        }
    }

    public static class AccountTeamRepositoryExtension
    {
        public static IQueryable<AccountTeam> Filter(
            this IQueryable<AccountTeam> AccountTeams,
            int id,
            int Fk_Account,
            int Fk_Season)

        {
            return AccountTeams.Where(a => (id == 0 || a.Id == id) &&
                                           (Fk_Account == 0 || a.Fk_Account == Fk_Account) &&
                                           (Fk_Season == 0 || a.Fk_Season == Fk_Season));
        }

    }
}
