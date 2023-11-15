CREATE PROCEDURE [dbo].[SP_UpdatePrivateLeagueMembersPointsAndRanking]
@PrivateLeagueId int
AS
BEGIN
    SET NOCOUNT ON;
    WITH CTE AS (
                                                       SELECT
                                                           plm.Id AS MemberId,
                                                           (
                                                               SELECT SUM(atgw.TotalPoints)
                                                               FROM [dbo].[AccountTeamGameWeaks] atgw
                                                               JOIN [dbo].[GameWeaks] gw ON atgw.Fk_GameWeak = gw.Id
                                                               JOIN [dbo].[AccountTeams] act ON act.Id = atgw.Fk_AccountTeam
                                                               WHERE plm.Fk_Account = act.Fk_Account
                                                               AND gw.Fk_Season = plgw.Fk_Season
                                                               AND gw._365_GameWeakIdValue >= plgw._365_GameWeakIdValue
                                                           ) AS NewPoints
                                                       FROM [dbo].[PrivateLeagueMembers] plm
                                                       JOIN [dbo].[PrivateLeagues] pl ON pl.Id = plm.Fk_PrivateLeague
                                                       JOIN [dbo].[GameWeaks] plgw ON plgw.Id = pl.Fk_GameWeak
                                                       WHERE plm.Fk_PrivateLeague = @PrivateLeagueId
                                                   )
                                                   UPDATE plm
                                                   SET
                                                       Points = CTE.NewPoints,
                                                       Ranking = (
                                                           SELECT COUNT(*) + 1
                                                           FROM CTE c
                                                           WHERE c.NewPoints > CTE.NewPoints
                                                       )
                                                   FROM [dbo].[PrivateLeagueMembers] plm
                                                   JOIN CTE ON plm.Id = CTE.MemberId;
                                                   
END
GO

