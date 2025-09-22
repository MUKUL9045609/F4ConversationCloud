
CREATE PROCEDURE [dbo].[sp_CheckUserExistsByMailId] 
	@email VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id]
		,[Email]
		,[Password]
		,[IsActive]
		,Stage
	FROM [dbo].[client_info]
	WHERE [Email] = @email
END