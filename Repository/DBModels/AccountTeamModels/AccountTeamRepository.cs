using Entities.CoreServicesModels.AccountTeamModels;
using Entities.DBModels.AccountTeamModels;
using Microsoft.Data.SqlClient;
using static Contracts.EnumData.DBModelsEnum;

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
                           parameters.HaveGoldSubscription,
                           parameters.FromGoldSubscriptionRanking,
                           parameters.FromUnSubscriptionRanking,
                           parameters.DashboardSearch,
                           parameters.Fk_AccountTeams,
                           parameters.Fk_CommunicationStatus,
                           parameters.Fk_CommunicationStatuses);
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
            entity.TwiceCaptain = 1;
            entity.Top_11 = 1;
            entity.TripleCaptain = 1;

            base.Create(entity);
        }

        public List<AccountTeamModel> GetPrivateLeaguesPoints(int fk_Season, int fk_PrivateLeague, int _365_GameWeakIdValue)
        {
            return FindByCondition(a => a.Fk_Season == fk_Season &&
                                        a.Account.PrivateLeagueMembers.Any(b => b.Fk_PrivateLeague == fk_PrivateLeague) &&
                                        a.AccountTeamGameWeaks.Any(b => b.GameWeak._365_GameWeakIdValue >= _365_GameWeakIdValue), trackChanges: false)
                   .Select(a => new AccountTeamModel
                   {
                       Fk_Account = a.Fk_Account,
                       TotalPoints = a.AccountTeamGameWeaks
                                      .Where(b => b.GameWeak._365_GameWeakIdValue >= _365_GameWeakIdValue)
                                      .Select(b => b.TotalPoints ?? 0)
                                      .Sum()
                   })
                   .OrderByDescending(a => a.TotalPoints)
                   .ToList();
        }

        public void UpdateAllAccountTeamsRanking(int fk_Season)
        {
            _ = DBContext.Database.ExecuteSqlRaw("EXEC [dbo].[SP_UpdateAccountTeamGlobalRanking] @SeasonId", new SqlParameter("@SeasonId", fk_Season));
            _ = DBContext.Database.ExecuteSqlRaw("EXEC [dbo].[SP_UpdateAccountTeamFavouriteTeamRanking] @SeasonId", new SqlParameter("@SeasonId", fk_Season));
            _ = DBContext.Database.ExecuteSqlRaw("EXEC [dbo].[SP_UpdateAccountTeamCountryRanking] @SeasonId", new SqlParameter("@SeasonId", fk_Season));
            _ = DBContext.Database.ExecuteSqlRaw("EXEC [dbo].[SP_UpdateAccountTeamGoldSubscriptionRanking] @SeasonId", new SqlParameter("@SeasonId", fk_Season));
            _ = DBContext.Database.ExecuteSqlRaw("EXEC [dbo].[SP_UpdateAccountTeamUnSubscriptionRanking] @SeasonId", new SqlParameter("@SeasonId", fk_Season));

        }

        public void UpdateAccountTeamPoints(int fk_AccountTeam, int fk_Season)
        {
            _ = DBContext.Database.ExecuteSqlRaw(@"UPDATE act
                                                   SET act.TotalPoints = (
                                                       SELECT SUM(atgw.TotalPoints)
                                                       FROM [dbo].[AccountTeamGameWeaks] atgw
                                                       JOIN [dbo].[GameWeaks] gw ON atgw.Fk_GameWeak = gw.Id
                                                       WHERE atgw.Fk_AccountTeam = act.Id
                                                       AND gw.Fk_Season = @SeasonId
                                                   )
                                                   FROM [dbo].[AccountTeams] act
                                                   WHERE act.Id = @AccountTeamId",
                                                   new SqlParameter("@AccountTeamId", fk_AccountTeam),
                                                   new SqlParameter("@SeasonId", fk_Season));
        }

		public void UpdateAccountTeamGold(int fk_AccountTeam, int fk_Season)
		{
			_ = DBContext.Database.ExecuteSqlRaw(@"UPDATE act
                                                   SET act.HaveGoldSubscription = CASE
                                                       WHEN EXISTS (
                                                           SELECT 1
                                                           FROM [dbo].[AccountSubscriptions] AS asub
                                                           WHERE asub.Fk_Subscription = 10
                                                           AND asub.Fk_Season = @SeasonId
                                                           AND asub.IsActive = 1
                                                       ) THEN 1
                                                       ELSE 0
                                                       END
                                                   FROM [dbo].[Accounts] AS act
                                                   WHERE act.Id = @AccountTeamId;",
												   new SqlParameter("@AccountTeamId", fk_AccountTeam),
												   new SqlParameter("@SeasonId", fk_Season));
		}

		public void UpdateTotalTeamPrice(int fk_AccountTeam, int fk_GameWeak)
        {
            _ = DBContext.Database.ExecuteSqlRaw(@"UPDATE AccountTeams
                                                   SET TotalTeamPrice = COALESCE((
                                                       SELECT SUM(LastSellPrice) AS TotalSellPrice
                                                       FROM (
                                                           SELECT
                                                               pp.SellPrice AS LastSellPrice,
                                                               ROW_NUMBER() OVER (PARTITION BY pp.Fk_Player ORDER BY pp.Id DESC) AS RowNum
                                                           FROM AccountTeamPlayers atp
                                                           INNER JOIN PlayerPrices pp ON atp.Fk_Player = pp.Fk_Player
                                                           WHERE atp.Fk_AccountTeam = @AccountTeamId
                                                           AND EXISTS (
                                                               SELECT 1
                                                               FROM AccountTeamPlayerGameWeaks atpgw
                                                               INNER JOIN GameWeaks gw ON atpgw.Fk_GameWeak = gw.Id
                                                               INNER JOIN AccountTeamPlayers acpl ON atpgw.Fk_AccountTeamPlayer = acpl.Id
                                                               WHERE acpl.Fk_AccountTeam = atp.Fk_AccountTeam
                                                               AND acpl.Fk_Player = atp.Fk_Player
                                                               AND gw.Id = @GameWeakId
                                                               AND gw.IsNext = 1
                                                               AND atpgw.IsTransfer = 0
                                                           )
                                                       ) AS Subquery
                                                       WHERE RowNum = 1
                                                   ),0)
                                                   WHERE Id = @AccountTeamId;
                                                   ", new SqlParameter("@AccountTeamId", fk_AccountTeam)
                                                   , new SqlParameter("@GameWeakId", fk_GameWeak));
        }

        public void UpdateAccountTeamUpdateCards(AccountTeamsUpdateCards updateCards)
        {
            if (updateCards.Fk_AccounTeam > 0)
            {
                AccountTeam accounTeam = FindByCondition(a => a.Id == updateCards.Fk_AccounTeam, trackChanges: true).FirstOrDefault();

                if (accounTeam != null)
                {
                    UpdateAccountTeamUpdateCards(updateCards, accounTeam);
                }
            }
            else if (updateCards.Fk_AccounTeams != null && updateCards.Fk_AccounTeams.Any())
            {
                List<AccountTeam> accounTeams = FindByCondition(a => updateCards.Fk_AccounTeams.Contains(a.Id), trackChanges: true).ToList();

                accounTeams.ForEach(accounTeam => UpdateAccountTeamUpdateCards(updateCards, accounTeam));
            }
            else
            {
                List<AccountTeam> accounTeams = FindByCondition(a => a.AccountTeamGameWeaks.Any(), trackChanges: true).ToList();

                accounTeams.ForEach(accounTeam => UpdateAccountTeamUpdateCards(updateCards, accounTeam));
            }
            _ = DBContext.SaveChanges();
        }

        private void UpdateAccountTeamUpdateCards(AccountTeamUpdateCards updateCards, AccountTeam accountTeam)
        {
            if (updateCards.BenchBoost > 0)
            {
                accountTeam.BenchBoost++;
            }
            if (updateCards.FreeHit > 0)
            {
                accountTeam.FreeHit++;
            }
            if (updateCards.WildCard > 0)
            {
                accountTeam.WildCard++;
            }
            if (updateCards.DoubleGameWeak > 0)
            {
                accountTeam.DoubleGameWeak++;
            }
            if (updateCards.Top_11 > 0)
            {
                accountTeam.Top_11++;
            }
            if (updateCards.TwiceCaptain > 0)
            {
                accountTeam.TwiceCaptain++;
            }
            if (updateCards.FreeTransfer > 0)
            {
                accountTeam.FreeTransfer++;
            }
            if (updateCards.TripleCaptain > 0)
            {
                accountTeam.TripleCaptain++;
            }
        }

        public void TransfareFavouriteTeam()
        {
            List<AccountTeam> accountTeams = FindByCondition(a => a.Account.Fk_FavouriteTeam != null, trackChanges: true)
                               .Include(a => a.Account)
                               .ToList();

            foreach (AccountTeam accountTeam in accountTeams)
            {
                accountTeam.Fk_FavouriteTeam = accountTeam.Account.Fk_FavouriteTeam.Value;
            }
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
            bool? HaveGoldSubscription,
            int? FromGoldSubscriptionRanking,
            int? FromUnSubscriptionRanking,
            string dashboardSearch,
            List<int> fk_AccountTeams,
            int? fk_CommunicationStatus,
            List<int> fk_CommunicationStatuses)

        {
            return AccountTeams.Where(a => (id == 0 || a.Id == id) &&

                                           (string.IsNullOrEmpty(dashboardSearch) ||
                                            a.Id.ToString().Contains(dashboardSearch) ||
                                            a.Account.FullName.Contains(dashboardSearch) ||
                                            a.Name.Contains(dashboardSearch)) &&

                                           (Fk_GameWeak == 0 || a.AccountTeamGameWeaks.Any(a => a.Fk_GameWeak == Fk_GameWeak)) &&
                                           (pointsFrom == null || a.TotalPoints >= pointsFrom) &&
                                           (pointsTo == null || a.TotalPoints <= pointsTo) &&

                                           (CurrentSeason == null || a.Season.IsCurrent == CurrentSeason) &&
                                           (FromTotalPoints == null || a.TotalPoints >= FromTotalPoints) &&

                                           (FromGlobalRanking == null || a.GlobalRanking >= FromGlobalRanking) &&
                                           (HaveGoldSubscription == null || (HaveGoldSubscription == true ? a.Account
                                                                              .AccountSubscriptions
                                                                              .Any(b => b.Fk_Subscription == (int)SubscriptionEnum.Gold &&
                                                                                        b.IsActive &&
                                                                                        (Fk_Season == 0 || b.Fk_Season == Fk_Season)) : !a.Account
                                                                              .AccountSubscriptions
                                                                              .Any(b => b.Fk_Subscription == (int)SubscriptionEnum.Gold &&
                                                                                        b.IsActive &&
                                                                                        (Fk_Season == 0 || b.Fk_Season == Fk_Season)))) &&
                                           (FromGoldSubscriptionRanking == null || a.GoldSubscriptionRanking >= FromGoldSubscriptionRanking) &&
                                           (FromUnSubscriptionRanking == null || a.UnSubscriptionRanking >= FromUnSubscriptionRanking) &&

                                           (Fk_Country == 0 || a.Account.Fk_Country == Fk_Country) &&
                                           (Fk_FavouriteTeam == 0 || a.Fk_FavouriteTeam == Fk_FavouriteTeam) &&
                                           (Fk_Account == 0 || a.Fk_Account == Fk_Account) &&
                                           (Fk_User == 0 || a.Account.Fk_User == Fk_User) &&
                                           (Fk_PrivateLeague == 0 || a.Account.PrivateLeagueMembers.Any(b => b.Fk_PrivateLeague == Fk_PrivateLeague)) &&
                                           (CreatedAtFrom == null || a.CreatedAt >= CreatedAtFrom) &&
                                           (CreatedAtTo == null || a.CreatedAt <= CreatedAtTo) &&
                                           (string.IsNullOrWhiteSpace(AccountUserName) || a.Account.User.UserName.ToLower().Contains(AccountUserName)) &&
                                           (string.IsNullOrWhiteSpace(AccountFullName) || a.Account.FullName.ToLower().Contains(AccountFullName)) &&
                                           (Fk_Season == 0 || a.Fk_Season == Fk_Season) &&

                                           (fk_CommunicationStatus == null || a.Fk_CommunicationStatus == fk_CommunicationStatus) &&
                                           (fk_CommunicationStatuses == null || !fk_CommunicationStatuses.Any() ||
                                                fk_CommunicationStatuses.Contains((int)a.Fk_CommunicationStatus)) &&

                                           (fk_AccountTeams == null || !fk_AccountTeams.Any() || fk_AccountTeams.Contains(a.Id)));
        }

    }
}
