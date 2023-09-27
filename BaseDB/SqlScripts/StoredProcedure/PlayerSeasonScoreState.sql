CREATE PROCEDURE [dbo].[SP_UpdatePlayerSeasonScoreStateTop15]
@SeasonId int
AS
BEGIN
    SET NOCOUNT ON;
    WITH
        RankedItems
        AS
        (
            SELECT 
                pgm.Id, 
                pgm.Points, 
                ROW_NUMBER() OVER (ORDER BY pgm.Points DESC, pgm.Fk_Player DESC) AS NewRank
            FROM [dbo].[PlayerSeasonScoreStates] as pgm
            WHERE pgm.Fk_Season = @SeasonId
              AND pgm.Fk_ScoreState = 1 
        )
        UPDATE pgm
        SET pgm.Top15 = CASE
            WHEN gri.NewRank <= 15 THEN gri.NewRank
            ELSE NULL
            END
        FROM [dbo].[PlayerSeasonScoreStates] pgm
        JOIN RankedItems gri ON pgm.Id= gri.Id;
END
GO