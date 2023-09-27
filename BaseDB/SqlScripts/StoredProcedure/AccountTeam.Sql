CREATE PROCEDURE [dbo].[SP_UpdateAccountTeamGlobalRanking]
@SeasonId int
AS
BEGIN
    SET NOCOUNT ON;
    WITH
        RankedItems
        AS
        (
            SELECT Id, TotalPoints, ROW_NUMBER() OVER (ORDER BY TotalPoints DESC) AS NewRank
            FROM [dbo].[AccountTeams] as act
            WHERE act.Fk_Season = @SeasonId
        )
        UPDATE act
        SET act.GlobalRanking = gri.NewRank
        FROM [dbo].[AccountTeams] act
        JOIN RankedItems gri ON act.Id= gri.Id;
END
GO

CREATE PROCEDURE [dbo].[SP_UpdateAccountTeamFavouriteTeamRanking]
@SeasonId int
AS
BEGIN
    SET NOCOUNT ON;
    WITH
        RankedItems
        AS
        (
            SELECT Id, TotalPoints, ROW_NUMBER() OVER (PARTITION BY Fk_FavouriteTeam ORDER BY TotalPoints DESC) AS NewRank
            FROM [dbo].[AccountTeams] as act
            WHERE act.Fk_Season = @SeasonId
        )
        UPDATE act
        SET act.FavouriteTeamRanking = gri.NewRank
        FROM [dbo].[AccountTeams] act
        JOIN RankedItems gri ON act.Id= gri.Id;
END
GO

CREATE PROCEDURE [dbo].[SP_UpdateAccountTeamCountryRanking]
@SeasonId int
AS
BEGIN
    SET NOCOUNT ON;
    WITH
        RankedItems
        AS
        (
            SELECT 
                act.Id, 
                act.TotalPoints, 
                ROW_NUMBER() OVER (PARTITION BY acc.Fk_Country ORDER BY act.TotalPoints DESC) AS NewRank
            FROM [dbo].[AccountTeams] as act
            JOIN [dbo].[Accounts] as acc ON act.Fk_Account = acc.Id
            WHERE act.Fk_Season = @SeasonId
        )
    UPDATE act
    SET act.CountryRanking = gri.NewRank
    FROM [dbo].[AccountTeams] act
    JOIN RankedItems gri ON act.Id = gri.Id;
END
GO

CREATE PROCEDURE [dbo].[SP_UpdateAccountTeamGoldSubscriptionRanking]
@SeasonId int
AS
BEGIN
    SET NOCOUNT ON;
    WITH
        RankedItems
        AS
        (
            SELECT 
                act.Id, 
                act.TotalPoints, 
                ROW_NUMBER() OVER (ORDER BY act.TotalPoints DESC) AS NewRank
            FROM [dbo].[AccountTeams] as act
            JOIN [dbo].[AccountSubscriptions] AS subs ON act.Fk_Account = subs.Fk_Account
            WHERE act.Fk_Season = @SeasonId
              AND subs.isActive = 1 
              AND subs.Fk_Subscription = 10
              AND subs.Fk_Season = @SeasonId
        )
    UPDATE act
    SET act.GoldSubscriptionRanking = gri.NewRank
    FROM [dbo].[AccountTeams] act
    JOIN RankedItems gri ON act.Id = gri.Id;
END

