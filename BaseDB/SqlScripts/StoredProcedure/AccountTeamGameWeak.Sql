CREATE PROCEDURE [dbo].[SP_UpdateAccountTeamGameWeakGlobalRanking]
@GameWeakId int
AS
BEGIN
    SET NOCOUNT ON;
    WITH
        RankedItems
        AS
        (
            SELECT Id, TotalPoints, ROW_NUMBER() OVER (ORDER BY TotalPoints DESC) AS NewRank
            FROM [dbo].[AccountTeamGameWeaks] as act
            WHERE act.Fk_GameWeak = @GameWeakId
        )
        UPDATE act
        SET act.GlobalRanking = gri.NewRank
        FROM [dbo].[AccountTeamGameWeaks] act
        JOIN RankedItems gri ON act.Id= gri.Id;
END
GO

CREATE PROCEDURE [dbo].[SP_UpdateAccountTeamGameWeakFavouriteTeamRanking]
@GameWeakId int
AS
BEGIN
    SET NOCOUNT ON;
    WITH
        RankedItems
        AS
        (
            SELECT 
                atgw.Id, 
                atgw.TotalPoints, 
                ROW_NUMBER() OVER (PARTITION BY act.Fk_FavouriteTeam ORDER BY atgw.TotalPoints DESC) AS NewRank
            FROM [dbo].[AccountTeams] as act
            JOIN [dbo].[AccountTeamGameWeaks] as atgw ON act.Id = atgw.Fk_AccountTeam
            WHERE atgw.Fk_GameWeak = @GameWeakId
        )
    UPDATE atgw
    SET atgw.FavouriteTeamRanking = gri.NewRank
    FROM [dbo].[AccountTeamGameWeaks] atgw
    JOIN RankedItems gri ON atgw.Id = gri.Id;
END
GO


CREATE PROCEDURE [dbo].[SP_UpdateAccountTeamGameWeakCountryRanking]
@GameWeakId int
AS
BEGIN
    SET NOCOUNT ON;
    WITH
        RankedItems
        AS
        (
            SELECT 
                atgw.Id, 
                atgw.TotalPoints, 
                ROW_NUMBER() OVER (PARTITION BY acc.Fk_Country ORDER BY atgw.TotalPoints DESC) AS NewRank
            FROM [dbo].[AccountTeams] as act
            JOIN [dbo].[Accounts] as acc ON act.Fk_Account = acc.Id
            JOIN [dbo].[AccountTeamGameWeaks] as atgw ON act.Id = atgw.Fk_AccountTeam
            WHERE atgw.Fk_GameWeak = @GameWeakId
        )
    UPDATE atgw
    SET atgw.CountryRanking = gri.NewRank
    FROM [dbo].[AccountTeamGameWeaks] atgw
    JOIN RankedItems gri ON atgw.Id = gri.Id;
END
GO
GO

