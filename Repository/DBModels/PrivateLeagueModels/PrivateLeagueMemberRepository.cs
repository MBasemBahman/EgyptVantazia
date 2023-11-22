using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.DBModels.PrivateLeagueModels;
using Microsoft.Data.SqlClient;
using static Contracts.EnumData.DBModelsEnum;

namespace Repository.DBModels.PrivateLeagueModels
{
    public class PrivateLeagueMemberRepository : RepositoryBase<PrivateLeagueMember>
    {
        public PrivateLeagueMemberRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<PrivateLeagueMember> FindAll(PrivateLeagueMemberParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Account,
                           parameters.Fk_PrivateLeague,
                           parameters.IsAdmin,
                           parameters.Fk_Season,
                           parameters.FromPoints,
                           parameters.ToPoints,
                           parameters.HaveTeam,
                           parameters.IgnoreZeroPoints,
                           parameters.IgnoreGoldSubscription);
        }

        public async Task<PrivateLeagueMember> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

		public void UpdatePrivateLeagueMembersPointsAndRanking(int fk_PrivateLeague)
		{
			_ = DBContext.Database.ExecuteSqlRaw(@"WITH MemberPoints AS (
    SELECT
        plm.Id AS MemberId,
        SUM(atgw.TotalPoints) AS NewPoints
    FROM 
        [dbo].[PrivateLeagueMembers] plm
        JOIN [dbo].[PrivateLeagues] pl ON pl.Id = plm.Fk_PrivateLeague
		LEFT JOIN [dbo].[AccountTeams] act ON act.Fk_Account = plm.Fk_Account
        JOIN [dbo].[GameWeaks] plgw ON plgw.Id = pl.Fk_GameWeak
        LEFT JOIN [dbo].[AccountTeamGameWeaks] atgw ON atgw.Fk_AccountTeam = act.Id
        LEFT JOIN [dbo].[GameWeaks] gw ON atgw.Fk_GameWeak = gw.Id
    WHERE 
        plm.Fk_PrivateLeague = @PrivateLeagueId
        AND gw.Fk_Season = plgw.Fk_Season
        AND gw._365_GameWeakIdValue >= plgw._365_GameWeakIdValue
    GROUP BY
        plm.Id
)

UPDATE m
SET
    Points = mp.NewPoints,
    Ranking = c.Rank
FROM 
    [dbo].[PrivateLeagueMembers] m
    JOIN MemberPoints mp ON m.Id = mp.MemberId
    CROSS APPLY (
        SELECT COUNT(*) + 1 AS Rank
        FROM MemberPoints c
        WHERE c.NewPoints > mp.NewPoints
    ) c;
",
												   new SqlParameter("@PrivateLeagueId", fk_PrivateLeague));
		}

		public new void Create(PrivateLeagueMember entity)
        {
            if (FindByCondition(a => a.Fk_PrivateLeague == entity.Fk_PrivateLeague && a.Fk_Account == entity.Fk_Account, trackChanges: false).Any())
            {
                PrivateLeagueMember oldEntity = FindByCondition(a => a.Fk_PrivateLeague == entity.Fk_PrivateLeague && a.Fk_Account == entity.Fk_Account, trackChanges: true).First();

                oldEntity.Ranking = entity.Ranking;
                oldEntity.Points = entity.Points;
            }
            else
            {
                base.Create(entity);
            }
        }
    }

    public static class PrivateLeagueMemberRepositoryExtension
    {
        public static IQueryable<PrivateLeagueMember> Filter(
            this IQueryable<PrivateLeagueMember> PrivateLeagueMembers,
            int id,
            int Fk_Account,
            int Fk_PrivateLeague,
            bool? IsAdmin,
            int Fk_Season,
            int? fromPoints,
            int? toPoints,
            bool haveTeam,
            bool ignoreZeroPoints,
            bool ignoreGoldSubscription)
        {
            return PrivateLeagueMembers.Where(a => (id == 0 || a.Id == id) &&
                                                   (haveTeam == false || a.Account.AccountTeams.Any(b => b.AccountTeamGameWeaks.Any())) &&
                                                   (ignoreZeroPoints == false || a.Points > 0) &&
                                                   (ignoreGoldSubscription == false || !a.Account.AccountSubscriptions
                                                                                         .Any(b => b.Fk_Subscription == (int)SubscriptionEnum.Gold &&
                                                                                                   b.IsActive)) &&
                                                   (fromPoints == null || a.Points >= fromPoints) &&
                                                   (toPoints == null || a.Points <= toPoints) &&
                                                   (Fk_Account == 0 || a.Fk_Account == Fk_Account) &&
                                                   (Fk_Season == 0 || a.Account.AccountTeams.Any(b => b.Fk_Season == Fk_Season)) &&
                                                   (IsAdmin == null || a.IsAdmin == IsAdmin) &&
                                                   (Fk_PrivateLeague == 0 || a.Fk_PrivateLeague == Fk_PrivateLeague));

        }

    }
}
