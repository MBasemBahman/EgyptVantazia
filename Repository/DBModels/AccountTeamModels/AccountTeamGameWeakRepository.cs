using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;
using Microsoft.Data.SqlClient;

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
                           parameters.TwiceCaptain,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo,
                           parameters.AccountFullName,
                           parameters.AccountUserName,
                           parameters.DashboardSearch,
                           parameters.Fk_Players,
                           parameters.Fk_Teams,
                           parameters.Fk_PrivateLeague,
                           parameters.UseCards);

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

        public void UpdateAllAccountTeamGameWeaksRanking(int fk_GameWeek)
        {
            _ = DBContext.Database.ExecuteSqlRaw("EXEC [dbo].[SP_UpdateAccountTeamGameWeakGlobalRanking] @GameWeakId", new SqlParameter("@GameWeakId", fk_GameWeek));
            _ = DBContext.Database.ExecuteSqlRaw("EXEC [dbo].[SP_UpdateAccountTeamGameWeakFavouriteTeamRanking] @GameWeakId", new SqlParameter("@GameWeakId", fk_GameWeek));
            _ = DBContext.Database.ExecuteSqlRaw("EXEC [dbo].[SP_UpdateAccountTeamGameWeakCountryRanking] @GameWeakId", new SqlParameter("@GameWeakId", fk_GameWeek));

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
            bool? TwiceCaptain,
            DateTime? CreatedAtFrom,
            DateTime? CreatedAtTo,
            string AccountFullName,
            string AccountUserName,
            string dashboardSearch,
            List<int> fk_Players,
            List<int> fk_Teams,
            int fk_PrivateLeague,
            bool? useCards)
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

                                                   (useCards == null ||
                                                    (useCards == true ?
                                                     (a.BenchBoost ||
                                                      a.FreeHit ||
                                                      a.WildCard ||
                                                      a.DoubleGameWeak ||
                                                      a.Top_11 ||
                                                      a.TripleCaptain ||
                                                      a.TwiceCaptain) :
                                                     !(a.BenchBoost ||
                                                      a.FreeHit ||
                                                      a.WildCard ||
                                                      a.DoubleGameWeak ||
                                                      a.Top_11 ||
                                                      a.TripleCaptain ||
                                                      a.TwiceCaptain))) &&

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
                                                   (TwiceCaptain == null || a.TwiceCaptain == TwiceCaptain) &&
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
