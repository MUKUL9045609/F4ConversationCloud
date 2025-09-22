
CREATE   PROCEDURE [dbo].[sp_CheckEmailExists] 
	@email VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id]
	    ,[FirstName]
		,[LastName]
		,[PhoneNumber]
		,[Email]
		,[Password]
		,[IsActive]
		,[Role]
	FROM [dbo].[UserDetails]
	WHERE [Email] = @email
END