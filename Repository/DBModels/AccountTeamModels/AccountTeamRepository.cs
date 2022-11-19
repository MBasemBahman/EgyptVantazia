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
                           parameters.Fk_User,
                           parameters.CurrentSeason,
                           parameters.Fk_PrivateLeague,
                           parameters.Fk_Season,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo,
                           parameters.AccountFullName,
                           parameters.AccountUserName,
                           parameters.Fk_Country,
                           parameters.Fk_FavouriteTeam);
        }

        public async Task<AccountTeam> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }
    }

    public static class AccountTeamRepositoryExtension
    {
        public static IQueryable<AccountTeam> Filter(
            this IQueryable<AccountTeam> AccountTeams,
            int id,
            int Fk_Account,
            int Fk_User,
            bool? CurrentSeason,
            int Fk_PrivateLeague,
            int Fk_Season,
            DateTime? CreatedAtFrom,
            DateTime? CreatedAtTo,
            string AccountFullName,
            string AccountUserName,
            int Fk_Country,
            int Fk_FavouriteTeam)

        {
            return AccountTeams.Where(a => (id == 0 || a.Id == id) &&
                                           (CurrentSeason == null || a.Season.IsCurrent == CurrentSeason) &&
                                           (Fk_Country == 0 || a.Account.Fk_Country == Fk_Country) &&
                                           (Fk_FavouriteTeam == 0 || a.Account.Fk_FavouriteTeam == Fk_FavouriteTeam) &&
                                           (Fk_Account == 0 || a.Fk_Account == Fk_Account) &&
                                           (Fk_User == 0 || a.Account.Fk_User == Fk_User) &&
                                           (Fk_PrivateLeague == 0 || a.Account.PrivateLeagueMembers.Any(b => b.Fk_PrivateLeague == Fk_PrivateLeague)) &&
                                           (CreatedAtFrom == null || a.CreatedAt >= CreatedAtFrom) &&
                                           (CreatedAtTo == null || a.CreatedAt <= CreatedAtTo) &&
                                           (string.IsNullOrWhiteSpace(AccountUserName) || a.Account.User.UserName.ToLower().Contains(AccountUserName)) &&
                                           (string.IsNullOrWhiteSpace(AccountFullName) || a.Account.FullName.ToLower().Contains(AccountFullName)) &&
                                           (Fk_Season == 0 || a.Fk_Season == Fk_Season));
        }

    }
}
