-- exec sp_CheckMailOrPhoneNumber null,'ashwin@gmail.com'
CREATE PROCEDURE [dbo].[sp_CheckMailOrPhoneNumber]
    @UserEmailId VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS(
        SELECT 1
        FROM client_info
        WHERE Email = @UserEmailId AND @UserEmailId IS NOT NULL)
    
    BEGIN
        SELECT 1 AS Result;   
    END
    ELSE
    BEGIN
        SELECT 0 AS Result;   
    END
END;