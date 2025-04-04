CREATE FUNCTION [dbo].[GET_POS]
(
    @ProductID INT,
    @Pos INT
)
RETURNS INT
AS
BEGIN
    DECLARE @ImageId INT;

    WITH RankedImages AS 
    (
        SELECT 
            ImageId,
            ProductId,
            ROW_NUMBER() OVER (PARTITION BY ProductId ORDER BY ImageId) AS pos
        FROM ProductImages
    )
    SELECT @ImageId = ImageId
    FROM RankedImages
    WHERE ProductId = @ProductID 
    AND pos = 
    (
        SELECT CASE 
            WHEN MAX(pos) >= @Pos THEN @Pos 
            ELSE 1 
        END
        FROM RankedImages
        WHERE ProductId = @ProductID
    );

    RETURN @ImageId;
END;
