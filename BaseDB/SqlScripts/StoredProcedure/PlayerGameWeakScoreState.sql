CREATE PROCEDURE [dbo].[SP_UpdatePlayerGameWeakScoreStateTop15]
@GameWeakId int
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
            FROM [dbo].[PlayerGameWeakScoreStates] as pgm
            WHERE pgm.Fk_GameWeak = @GameWeakId
              AND pgm.Fk_ScoreState = 1 
        )
        UPDATE pgm
        SET pgm.Top15 = CASE
            WHEN gri.NewRank <= 15 THEN gri.NewRank
            ELSE NULL
            END
        FROM [dbo].[PlayerGameWeakScoreStates] pgm
        JOIN RankedItems gri ON pgm.Id= gri.Id;
END
GO