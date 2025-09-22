CREATE PROCEDURE [dbo].[sp_RegisterNewUser]
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @Password VARCHAR(100) = NULL,
    @Email NVARCHAR(100) = NULL,
    @PhoneNumber VARCHAR(15) = NULL,
    @Address NVARCHAR(MAX) = NULL,
    @Country VARCHAR(100) = NULL,
    @Role VARCHAR(30),
	@ClientTimeZone Nvarchar(100),
	@Stage int

AS
BEGIN
    SET NOCOUNT ON;
	 DECLARE @NewUserId INT;
    BEGIN TRY
        INSERT INTO client_info(
            FirstName,
            LastName,
            Email,
            Password,
            IsActive,       
            CreatedOn,
            PhoneNumber,
            Address,
            Country,
            Role,
			ClientTimeZone,
			Stage
        )
        VALUES (
            @FirstName,
            @LastName,
            @Email,
            @Password,
            1,
            GETDATE(),
            @PhoneNumber,
            @Address,
            @Country,
            @Role,
			@ClientTimeZone,
			@Stage
        );

      
      
        SET @NewUserId = SCOPE_IDENTITY();

       
        SELECT  @NewUserId AS NewUserId;
    END TRY
    BEGIN CATCH
       
        SELECT 0 AS StatusCode, NULL AS NewUserId;
        RETURN;
    END CATCH
END