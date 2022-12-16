using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;


namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamRepository : RepositoryBase<AccountTeam>
    {
        public AccountTeamRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<AccountTeam> FindAll(AccountTeamParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Account,
                           parameters.Fk_GameWeak,
                           parameters.PointsFrom,
                           parameters.PointsTo,
                           parameters.Fk_User,
                           parameters.CurrentSeason,
                           parameters.Fk_PrivateLeague,
                           parameters.Fk_Season,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo,
                           parameters.AccountFullName,
                           parameters.AccountUserName,
                           parameters.Fk_Country,
                           parameters.Fk_FavouriteTeam,
                           parameters.FromTotalPoints,
                           parameters.FromGlobalRanking,
                           parameters.DashboardSearch);
        }

        public async Task<AccountTeam> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(AccountTeam entity)
        {
            entity.TotalMoney = 100;
            entity.FreeTransfer = 0;

            entity.BenchBoost = 1;
            entity.FreeHit = 1;
            entity.WildCard = 2;
            entity.DoubleGameWeak = 1;
            entity.Top_11 = 1;
            entity.TripleCaptain = 1;

            base.Create(entity);
        }
    }

    public static class AccountTeamRepositoryExtension
    {
        public static IQueryable<AccountTeam> Filter(
            this IQueryable<AccountTeam> AccountTeams,
            int id,
            int Fk_Account,
            int Fk_GameWeak,
            double? pointsFrom,
            double? pointsTo,
            int Fk_User,
            bool? CurrentSeason,
            int Fk_PrivateLeague,
            int Fk_Season,
            DateTime? CreatedAtFrom,
            DateTime? CreatedAtTo,
            string AccountFullName,
            string AccountUserName,
            int Fk_Country,
            int Fk_FavouriteTeam,
            int? FromTotalPoints,
            int? FromGlobalRanking,
            string dashboardSearch)

        {
            return AccountTeams.Where(a => (id == 0 || a.Id == id) &&

                                           (string.IsNullOrEmpty(dashboardSearch) ||
                                            a.Id.ToString().Contains(dashboardSearch) ||
                                            a.Account.FullName.Contains(dashboardSearch) ||
                                            a.Season.Name.Contains(dashboardSearch) ||
                                            a.Name.Contains(dashboardSearch)) &&

                                           (Fk_GameWeak == 0 || a.AccountTeamGameWeaks.Any(a => a.Fk_GameWeak == Fk_GameWeak) ) &&
                                           (pointsFrom == null || a.TotalPoints >= pointsFrom) &&
                                           (pointsTo == null || a.TotalPoints <= pointsTo) &&
                                           
                                           (CurrentSeason == null || a.Season.IsCurrent == CurrentSeason) &&
                                           (FromTotalPoints == null || a.TotalPoints >= FromTotalPoints) &&
                                           (FromGlobalRanking == null || a.GlobalRanking >= FromGlobalRanking) &&
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
