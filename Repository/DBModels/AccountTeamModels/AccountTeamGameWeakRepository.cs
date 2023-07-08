using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;

namespace Repository.DBModels.AccountTeamModels
{
    public class AccountTeamGameWeakRepository : RepositoryBase<AccountTeamGameWeak>
    {
        public AccountTeamGameWeakRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<AccountTeamGameWeak> FindAll(AccountTeamGameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_AccountTeam,
                           parameters.Fk_GameWeak,
                           parameters.PointsFrom,
                           parameters.PointsTo,
                           parameters.Fk_Season,
                           parameters.Fk_Account,
                           parameters._365_GameWeakId,
                           parameters.GameWeakFrom,
                           parameters.GameWeakTo,
                           parameters.BenchBoost,
                           parameters.FreeHit,
                           parameters.WildCard,
                           parameters.DoubleGameWeak,
                           parameters.Top_11,
                           parameters.TripleCaptain,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo,
                           parameters.AccountFullName,
                           parameters.AccountUserName,
                           parameters.DashboardSearch,
                           parameters.Fk_Players,
                           parameters.Fk_Teams,
                           parameters.Fk_PrivateLeague);

        }

        public async Task<AccountTeamGameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public double GetAverageGameWeakPoints(int fk_GameWeak)
        {
            return FindByCondition(a => a.Fk_GameWeak == fk_GameWeak, trackChanges: false).Any() ?
                    FindByCondition(a => a.Fk_GameWeak == fk_GameWeak, trackChanges: false)
                   .Select(a => a.TotalPoints ?? 0)
                   .Average() : 0;
        }

        public new void Create(AccountTeamGameWeak entity)
        {
            if (!FindByCondition(a => a.Fk_AccountTeam == entity.Fk_AccountTeam && a.Fk_GameWeak == entity.Fk_GameWeak, trackChanges: false).Any())
            {
                AccountTeam accountTeam = DBContext.Set<AccountTeam>().Find(entity.Fk_AccountTeam);

                if (accountTeam != null)
                {
                    accountTeam.FreeTransfer = accountTeam.FreeTransfer >= 1 ? 2 : 1;

                    base.Create(entity);
                }
            }
        }

        public void UpdateRank(int fk_AccountTeam, int fk_GameWeek)
        {
            DateTime lasUpdate = DateTime.UtcNow.AddDays(-1).Date;

            var accountTeamModel = FindByCondition(a => a.Fk_AccountTeam == fk_AccountTeam && 
                                                        a.Fk_GameWeak == fk_GameWeek &&
                                                        (a.GlobalRankingUpdatedAt < lasUpdate ||
                                                         a.CountryRankingUpdatedAt < lasUpdate ||
                                                         a.FavouriteTeamRankingUpdatedAt < lasUpdate), trackChanges: false)
                                   .Select(a => new
                                   {
                                       a.Id,

                                       a.GlobalRanking,
                                       a.GlobalRankingUpdatedAt,

                                       a.CountryRanking,
                                       a.CountryRankingUpdatedAt,

                                       a.FavouriteTeamRanking,
                                       a.FavouriteTeamRankingUpdatedAt,

                                       a.AccountTeam.Account.Fk_Country,
                                       a.AccountTeam.Fk_FavouriteTeam

                                   }).FirstOrDefault();

            if (accountTeamModel == null)
            {
                return;
            }

            var accounts = FindByCondition(a => a.Fk_GameWeak == fk_GameWeek, trackChanges: false)
                           .OrderByDescending(a => a.TotalPoints)
                           .Select(a => new
                           {
                               a.Id,
                               a.TotalPoints,
                               a.AccountTeam.Fk_FavouriteTeam,
                               a.AccountTeam.Account.Fk_Country
                           })
                           .ToList();

            AccountTeamGameWeak accountTeam = FindById(accountTeamModel.Id, trackChanges: true).Result;

            if (accountTeamModel.GlobalRankingUpdatedAt == null ||
                accountTeamModel.GlobalRankingUpdatedAt < lasUpdate)
            {
                int rank = accounts
                           .Select((item, index) => new
                           {
                               item.Id,
                               index
                           })
                           .Where(a => a.Id == accountTeamModel.Id)
                           .FirstOrDefault().index + 1;

                if (rank != accountTeamModel.GlobalRanking)
                {
                    accountTeam.GlobalRanking = rank;
                    accountTeam.GlobalRankingUpdatedAt = DateTime.UtcNow;
                }
            }

            if (accountTeamModel.CountryRankingUpdatedAt == null ||
               accountTeamModel.CountryRankingUpdatedAt < lasUpdate)
            {
                int rank = accounts
                           .Where(a => a.Fk_Country == accountTeamModel.Fk_Country)
                           .Select((item, index) => new
                           {
                               item.Id,
                               index
                           })
                           .Where(a => a.Id == accountTeamModel.Id)
                           .FirstOrDefault().index + 1;

                if (rank != accountTeamModel.CountryRanking)
                {
                    accountTeam.CountryRanking = rank;
                    accountTeam.CountryRankingUpdatedAt = DateTime.UtcNow;
                }
            }

            if (accountTeamModel.FavouriteTeamRankingUpdatedAt == null ||
               accountTeamModel.FavouriteTeamRankingUpdatedAt < lasUpdate)
            {
                int rank = accounts
                           .Where(a => a.Fk_FavouriteTeam == accountTeamModel.Fk_FavouriteTeam)
                           .Select((item, index) => new
                           {
                               item.Id,
                               index
                           })
                           .Where(a => a.Id == accountTeamModel.Id)
                           .FirstOrDefault().index + 1;

                if (rank != accountTeamModel.FavouriteTeamRanking)
                {
                    accountTeam.FavouriteTeamRanking = rank;
                    accountTeam.FavouriteTeamRankingUpdatedAt = DateTime.UtcNow;
                }
            }


            _ = DBContext.SaveChanges();
        }
    }

    public static class AccountTeamGameWeakRepositoryExtension
    {
        public static IQueryable<AccountTeamGameWeak> Filter(
            this IQueryable<AccountTeamGameWeak> AccountTeamGameWeaks,
            int id,
            int Fk_AccountTeam,
            int Fk_GameWeak,
            double? pointsFrom,
            double? pointsTo,
            int Fk_Season,
            int Fk_Account,
            string _365_GameWeakId,
            int GameWeakFrom,
            int GameWeakTo,
            bool? BenchBoost,
            bool? FreeHit,
            bool? WildCard,
            bool? DoubleGameWeak,
            bool? Top_11,
            bool? TrippleCaptain,
            DateTime? CreatedAtFrom,
            DateTime? CreatedAtTo,
            string AccountFullName,
            string AccountUserName,
            string dashboardSearch,
            List<int> fk_Players,
            List<int> fk_Teams,
            int fk_PrivateLeague)
        {
            return AccountTeamGameWeaks.Where(a => (id == 0 || a.Id == id) &&

                                                   (string.IsNullOrEmpty(dashboardSearch) ||
                                                    a.Id.ToString().Contains(dashboardSearch) ||
                                                    a.AccountTeam.Account.FullName.Contains(dashboardSearch) ||
                                                    a.AccountTeam.Season.Name.Contains(dashboardSearch) ||
                                                    a.AccountTeam.Name.Contains(dashboardSearch)) &&

                                                   (fk_Players == null ||
                                                    !fk_Players.Any() ||
                                                    a.AccountTeam
                                                     .AccountTeamPlayers
                                                     .Any(b => fk_Players.Contains(b.Fk_Player) &&
                                                               (Fk_GameWeak == 0 || b.AccountTeamPlayerGameWeaks.Any(c => c.Fk_GameWeak == Fk_GameWeak && c.IsTransfer == false)))) &&

                                                   (fk_Teams == null ||
                                                    !fk_Teams.Any() ||
                                                    a.AccountTeam
                                                     .AccountTeamPlayers
                                                     .Any(b => fk_Teams.Contains(b.Player.Fk_Team) &&
                                                               (Fk_GameWeak == 0 || b.AccountTeamPlayerGameWeaks.Any(c => c.Fk_GameWeak == Fk_GameWeak && c.IsTransfer == false)))) &&

                                                   (Fk_AccountTeam == 0 || a.Fk_AccountTeam == Fk_AccountTeam) &&

                                                   (fk_PrivateLeague == 0 || a.AccountTeam.Account.PrivateLeagueMembers.Any(b => b.Fk_PrivateLeague == fk_PrivateLeague)) &&

                                                   (GameWeakFrom == 0 || a.GameWeak._365_GameWeakIdValue >= GameWeakFrom) &&
                                                   (GameWeakTo == 0 || a.GameWeak._365_GameWeakIdValue <= GameWeakTo) &&
                                                   (pointsFrom == null || a.TotalPoints >= pointsFrom) &&
                                                   (pointsTo == null || a.TotalPoints <= pointsTo) &&
                                                   (BenchBoost == null || a.BenchBoost == BenchBoost) &&
                                                   (FreeHit == null || a.FreeHit == FreeHit) &&
                                                   (WildCard == null || a.WildCard == WildCard) &&
                                                   (DoubleGameWeak == null || a.DoubleGameWeak == DoubleGameWeak) &&
                                                   (Top_11 == null || a.Top_11 == Top_11) &&
                                                   (TrippleCaptain == null || a.TripleCaptain == TrippleCaptain) &&
                                                   (string.IsNullOrEmpty(_365_GameWeakId) || a.GameWeak._365_GameWeakId == _365_GameWeakId) &&
                                                   (Fk_Season == 0 || a.AccountTeam.Fk_Season == Fk_Season) &&
                                                   (Fk_Account == 0 || a.AccountTeam.Fk_Account == Fk_Account) &&
                                                   (CreatedAtFrom == null || a.CreatedAt >= CreatedAtFrom) &&
                                                   (CreatedAtTo == null || a.CreatedAt <= CreatedAtTo) &&
                                                   (string.IsNullOrWhiteSpace(AccountUserName) || a.AccountTeam.Account.User.UserName.ToLower().Contains(AccountUserName)) &&
                                                   (string.IsNullOrWhiteSpace(AccountFullName) || a.AccountTeam.Account.FullName.ToLower().Contains(AccountFullName)) &&
                                                   (Fk_GameWeak == 0 || a.Fk_GameWeak == Fk_GameWeak));

        }

    }
}
