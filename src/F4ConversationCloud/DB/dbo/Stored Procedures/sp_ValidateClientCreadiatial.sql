-- exec sp_ValidateClientCreadiatial 'ashwinchandode199@gmail.com'
CREATE PROCEDURE [dbo].[sp_ValidateClientCreadiatial]
	-- Add the parameters for the stored procedure here
	@EmailId VARCHAR(100)
	
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
			[Id] as UserId,
            Email,        
			Stage,
			password
	FROM [dbo].client_info
	WHERE Email = @EmailId
	AND IsActive = 1

 
END